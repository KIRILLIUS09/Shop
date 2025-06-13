using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class CartItem
    {
        
        public int CartItemId { get; set; }

       
        public int Quantity { get; set; } = 1;

       
        public int ProductId { get; set; }
        
        public int UserId { get; set; } 


        public Product Product { get; set; } = null!;

        public int? OrderId { get; set; }

        public Order? Order { get; set; }
    }
}
