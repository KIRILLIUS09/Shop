using Core.Contracts;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace orm.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IAppDbContext _context;

        public AddressRepository(IAppDbContext context) => _context = context;

        public async Task<Address?> GetByIdAsync(int addressId)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId);
        }

        public async Task<Address> AddAsync(Address address)
        {
            _context.AddEntity(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<List<Address>> GetByUserIdAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(int addressId)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId);

            if (address == null) return false;

            _context.RemoveEntity(address);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}