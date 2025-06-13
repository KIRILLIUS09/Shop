namespace BisLogic.Services
{
    public interface IAddressService
    {
        Task<Core.Models.Address> AddAddressAsync(Core.Models.Address address);
        Task<List<Core.Models.Address>> GetUserAddressesAsync(int userId);
        Task<bool> DeleteAddressAsync(int addressId);
        Task<Core.Models.Address> GetAddressByIdAsync(int addressId);
    }
}