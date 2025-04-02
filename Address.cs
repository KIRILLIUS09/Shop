using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace orm.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Region { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string Street { get; set; }

        [Required]
        [MaxLength(20)]
        public string Building { get; set; }

        
        [MaxLength(10)]
        public string Apartment { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User User { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}