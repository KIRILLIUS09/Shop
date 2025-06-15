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
        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }

        [Required]
        public Address Address { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        public User User { get; set; } = null!;

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.New;

        [Required]
        public decimal TotalAmount { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public List<CartItem> CartItems { get; set; } = new();
    }
}
