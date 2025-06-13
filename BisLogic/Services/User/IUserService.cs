using System.Threading.Tasks;
using Core.Models;

namespace BisLogic.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(User user);
        Task<User> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task<Address> AddUserAddressAsync(int userId, Address address);
    }
}
