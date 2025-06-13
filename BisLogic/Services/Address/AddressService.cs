using Core.Contracts;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace BisLogic.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AddressService> _logger;

        public AddressService(
            IAddressRepository addressRepository,
            IUserRepository userRepository,
            ILogger<AddressService> logger)
        {
            _addressRepository = addressRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Address> AddAddressAsync(Address address)
        {
            try
            {
                // Проверяем существование пользователя
                var user = await _userRepository.GetUserByIdAsync(address.UserId ?? 0);
                if (user == null)
                    throw new KeyNotFoundException("Пользователь не найден");

                // Валидация адреса
                ValidateAddress(address);

                var createdAddress = await _addressRepository.AddAsync(address);
                _logger.LogInformation($"Добавлен новый адрес ID: {createdAddress.AddressId} для пользователя ID: {address.UserId}");

                return createdAddress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении адреса");
                throw;
            }
        }

        public async Task<List<Address>> GetUserAddressesAsync(int userId)
        {
            try
            {
                var addresses = await _addressRepository.GetByUserIdAsync(userId);
                return addresses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении адресов пользователя ID: {userId}");
                throw;
            }
        }

        public async Task<bool> DeleteAddressAsync(int addressId)
        {
            try
            {
                var result = await _addressRepository.DeleteAsync(addressId);
                if (result)
                    _logger.LogInformation($"Адрес ID: {addressId} удален");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении адреса ID: {addressId}");
                throw;
            }
        }

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            try
            {
                var address = await _addressRepository.GetByIdAsync(addressId);
                if (address == null)
                    throw new KeyNotFoundException($"Адрес ID: {addressId} не найден");

                return address;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении адреса ID: {addressId}");
                throw;
            }
        }

        private void ValidateAddress(Address address)
        {
            if (string.IsNullOrWhiteSpace(address.Region))
                throw new ArgumentException("Регион обязателен для заполнения");

            if (string.IsNullOrWhiteSpace(address.City))
                throw new ArgumentException("Город обязателен для заполнения");

            if (string.IsNullOrWhiteSpace(address.Street))
                throw new ArgumentException("Улица обязательна для заполнения");

            if (string.IsNullOrWhiteSpace(address.Building))
                throw new ArgumentException("Номер дома обязателен для заполнения");
        }
    }
}
