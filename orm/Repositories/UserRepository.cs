using Core.Contracts;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace orm.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IAppDbContext _context;

        public UserRepository(IAppDbContext context) => _context = context;

        public async Task<User> CreateUserAsync(User user)
        {
            _context.AddEntity(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Addresses)
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.UpdateEntity(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}