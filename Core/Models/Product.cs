using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Color { get; set; }

        [Required]
        [MaxLength(20)]
        public required string Size { get; set; }

        public List<CartItem> CartItems { get; set; } = [];
    }
}
