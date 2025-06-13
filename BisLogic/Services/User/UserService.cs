using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Core.Models;
using Core.Contracts;

namespace BisLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IAddressRepository addressRepository,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _logger = logger;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            try
            {
                // Валидация данных
                ValidateUser(user);

                // Проверка уникальности email
                if (await _userRepository.GetUserByEmailAsync(user.Email) != null)
                    throw new ArgumentException("Пользователь с таким email уже существует");

                // Хеширование пароля
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

                // Создание пользователя
                var createdUser = await _userRepository.CreateUserAsync(user);
                _logger.LogInformation($"Зарегистрирован новый пользователь ID: {createdUser.UserId}");
                return createdUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя");
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                    throw new KeyNotFoundException($"Пользователь с ID {id} не найден");

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении пользователя ID: {id}");
                throw;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                // Валидация данных
                ValidateUser(user);

                await _userRepository.UpdateUserAsync(user);
                _logger.LogInformation($"Обновлены данные пользователя ID: {user.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении пользователя ID: {user.UserId}");
                throw;
            }
        }

        public async Task<Address> AddUserAddressAsync(int userId, Address address)
        {
            try
            {
                // Проверка существования пользователя
                if (await _userRepository.GetUserByIdAsync(userId) == null)
                    throw new KeyNotFoundException("Пользователь не найден");

                // Валидация адреса
                ValidateAddress(address);

                // Установка связи с пользователем
                address.UserId = userId;

                var createdAddress = await _addressRepository.AddAsync(address);
                _logger.LogInformation($"Добавлен адрес ID: {createdAddress.AddressId} для пользователя ID: {userId}");
                return createdAddress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при добавлении адреса для пользователя ID: {userId}");
                throw;
            }
        }

        private void ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || !Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Некорректный email");

            if (string.IsNullOrWhiteSpace(user.PasswordHash) || user.PasswordHash.Length < 8)
                throw new ArgumentException("Пароль должен содержать минимум 8 символов");

            if (string.IsNullOrWhiteSpace(user.FirstName))
                throw new ArgumentException("Имя обязательно для заполнения");

            if (string.IsNullOrWhiteSpace(user.LastName))
                throw new ArgumentException("Фамилия обязательна для заполнения");

            if (string.IsNullOrWhiteSpace(user.PhoneNumber))
                throw new ArgumentException("Телефон обязателен для заполнения");
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
