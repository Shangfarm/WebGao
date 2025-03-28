using System;
using System.Collections.Generic;

namespace lab04TT.Models
{
    public class Order
    {
        public int Id { get; set; }  // Mã đơn hàng
        public DateTime OrderDate { get; set; }  // Ngày đặt hàng
        public decimal TotalPrice { get; set; }  // Tổng tiền
        public List<OrderItem> Items { get; set; } = new List<OrderItem>(); // Danh sách sản phẩm trong đơn hàng
    }
}
