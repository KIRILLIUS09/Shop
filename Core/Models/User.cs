namespace Core.Models
{
    public class User
    {
        public int UserId { get; set; }

        
        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }

       
        public required string LastName { get; set; }

        
        public required string FirstName { get; set; }

       
        public required string MiddleName { get; set; }

       
        public required string Username { get; set; }

       
        public required string PasswordHash { get; set; }
        public string Role { get; set; } = "User";


        public List<Address> Addresses { get; set; } = [];
        public List<Order> Orders { get; set; } = [];
    }
}
