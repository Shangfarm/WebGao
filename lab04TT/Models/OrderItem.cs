namespace lab04TT.Models
{
    public class OrderItem
    {
        public int Id { get; set; }  // Mã chi tiết đơn hàng
        public int OrderId { get; set; }  // Liên kết với Order
        public int ProductId { get; set; }  // Mã sản phẩm
        public string Name { get; set; }  // Tên sản phẩm
        public decimal Price { get; set; }  // Giá sản phẩm
        public int Quantity { get; set; }  // Số lượng mua

        public Order Order { get; set; }  // Khóa ngoại liên kết với Order
    }
}
