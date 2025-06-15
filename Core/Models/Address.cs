namespace Core.Models
{
    public class Address
    {
        
        public int AddressId { get; set; }

        
        public string Region { get; set; } = string.Empty;

       
        public string City { get; set; } = string.Empty;

        
        public string Street { get; set; } = string.Empty;

       
        public string Building { get; set; } = string.Empty;

        public string? Apartment { get; set; }

        
        public int? UserId { get; set; }

        public User? User { get; set; }


    }
}
