namespace Core.Models
{
    public class Product
    {
       
        public int ProductId { get; set; }

       
        public required string Name { get; set; }

        public decimal Price { get; set; }

       
        public required string Description { get; set; }

       
        public required string Color { get; set; }

        public required string Size { get; set; }

        public List<CartItem> CartItems { get; set; } = [];
    }
}
