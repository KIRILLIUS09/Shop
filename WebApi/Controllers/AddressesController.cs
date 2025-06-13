using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using orm;
using Core.Models;
using WebApi.DTOs.Requests.Address;
using WebApi.DTOs.Responses.Address;
using System.Linq;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController : BaseController
    {
        public AddressesController(AppDbContext context) : base(context) { }

        // POST: api/Addresses
        [HttpPost]
        public async Task<ActionResult<AddressResponse>> CreateAddress(
            [FromBody] AddressCreateRequest request)
        {
            try
            {
                // Проверяем существование пользователя
                var user = await _context.Users.FindAsync(request.UserId);
                if (user == null)
                    return BadRequest("Пользователь не найден");

                var address = new Address
                {
                    Region = request.Region,
                    City = request.City,
                    Street = request.Street,
                    Building = request.Building,
                    Apartment = request.Apartment,
                    UserId = request.UserId
                };

                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                var response = new AddressResponse(
                    address.AddressId,
                    $"{address.Region}, {address.City}, {address.Street} {address.Building}",
                    address.Region,
                    address.City,
                    address.Street,
                    address.Building,
                    address.Apartment,
                    address.UserId
                );

                return CreatedAtAction(nameof(GetAddress), new { id = address.AddressId }, response);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Ошибка при создании адреса");
            }
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressShortResponse>>> GetAllAddresses()
        {
            var addresses = await _context.Addresses
                .Select(a => new AddressShortResponse(
                    a.AddressId,
                    $"{a.City}, {a.Street} {a.Building}",
                    a.UserId
                ))
                .ToListAsync();

            return Ok(addresses);
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressResponse>> GetAddress(int id)
        {
            try
            {
                var address = await _context.Addresses.FindAsync(id);
                if (address == null)
                    return NotFound();

                
                var response = new AddressResponse(
                    address.AddressId,
                    $"{address.Region}, {address.City}, {address.Street} {address.Building}",
                    address.Region,
                    address.City,
                    address.Street,
                    address.Building,
                    address.Apartment,
                    address.UserId ?? 0 
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Ошибка при получении адреса");
            }
        }
    }
}