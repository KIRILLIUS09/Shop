using Core.Models;

namespace Core.Contracts
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(User user);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
