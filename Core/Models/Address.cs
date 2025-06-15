using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }

        [Required]
        [MaxLength(50)] 
        public string Region { get; set; } = string.Empty;

        [Required]
        [MaxLength(85)] 
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(120)] 
        public string Street { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)] 
        public string Building { get; set; } = string.Empty;

        [MaxLength(10)] 
        public string? Apartment { get; set; }

        [ForeignKey(nameof(User))]
        public int? UserId { get; set; }

        public User? User { get; set; }
    }
}
