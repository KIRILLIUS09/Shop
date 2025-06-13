using Core.Models;

namespace Core.Contracts
{
    public interface IAddressRepository
    {
        Task<Address> AddAsync(Address address);
        Task<List<Address>> GetByUserIdAsync(int userId);
        Task<bool> DeleteAsync(int addressId);
        Task<Address?> GetByIdAsync(int addressId); 
    }
}