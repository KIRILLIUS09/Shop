using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace orm.Models
{
    public enum OrderStatus
    {
        New,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")] 
        public OrderStatus Status { get; set; } = OrderStatus.New;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}