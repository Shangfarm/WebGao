using lab04TT.Models;
using Microsoft.EntityFrameworkCore;

namespace lab04TT.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
		public async Task<IEnumerable<Product>> GetAllAsync()
		{
			var products = await _context.Products.Include(p => p.Category).ToListAsync();

			if (products == null || !products.Any())
			{
				Console.WriteLine("Không có sản phẩm nào được lấy từ CSDL.");
			}
			else
			{
				Console.WriteLine($"Số sản phẩm lấy được: {products.Count()}");
			}

			return products;
		}

		public async Task<Product> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            return await _context.Products.Include(p =>p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
