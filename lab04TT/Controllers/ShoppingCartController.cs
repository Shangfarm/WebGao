using lab04TT.Extensions;
using lab04TT.Models;
using lab04TT.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab04TT.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        public ShoppingCartController(IProductRepository productRepository ,ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var product = await GetProductFromDatabase(productId);

            if (product == null)
            {
                return NotFound();
            }

            var cartItem = new CartItem
            {
                ProductId = productId,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity
            };

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.AddItem(cartItem);
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            // Chuyển hướng đến ShoppingCart trong HomeController
            return RedirectToAction("Index", "ShoppingCart");

        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ??
    new ShoppingCart();
            return View(cart);
        }
        // Các actions khác...
        private async Task<Product> GetProductFromDatabase(int productId)
        {
            // Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm
            var product = await _productRepository.GetByIdAsync(productId);
            return product;
        }
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart is not null)
            {
                cart.RemoveItem(productId);
                // Lưu lại giỏ hàng vào Session sau khi đã xóa mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Checkout() //xác nhân mua hàng 
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

            if (cart == null || !cart.Items.Any())
            {
                TempData["Message"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index");
            }

            // Tạo đơn hàng mới
            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity),
                Items = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Name = i.Name,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            };

            // Lưu vào database
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Xóa giỏ hàng sau khi thanh toán
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("OrderSuccess", new { orderId = order.Id });
        }

        public IActionResult OrderSuccess(int orderId) // thông báo mua hàng thành công và trả về mua hàng 
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }

}
