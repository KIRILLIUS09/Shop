using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace orm.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [MaxLength(500)] 
        public string Description { get; set; }

        
        [MaxLength(50)]
        public string Color { get; set; }

       
        [MaxLength(20)]
        public string Size { get; set; }

        
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

       
        public enum StandardColor
        {
            Red,
            Blue,
            Green,
            Black,
            White,
            Yellow,
            Other
        }

        public enum ClothingSize
        {
            XS,
            S,
            M,
            L,
            XL,
            XXL
        }
    }
}