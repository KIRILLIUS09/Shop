using Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int AddressId { get; set; }

        [Required]
        public Address Address { get; set; } = null!;

        [Required]
        public int UserId { get; set; }

        [Required]
        public User User { get; set; } = null!;

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.New;

        public decimal TotalAmount { get; set; }

        public string? Comment { get; set; } 

        public List<CartItem> CartItems { get; set; } = new();
    }
}