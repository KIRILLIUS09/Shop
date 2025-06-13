using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using orm;
using WebApi.DTOs.Requests.User;
using WebApi.DTOs.Responses.User;


namespace WebApi.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(AppDbContext context) : base(context) { }

        

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(int id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Addresses)
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                    return NotFound();

                // Маппинг в DTO
                return Ok(new UserResponse(
                    user.UserId,
                    user.Email,
                    user.Username,
                    user.FirstName,
                    user.LastName,
                    user.PhoneNumber
                ));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Ошибка при получении пользователя");
            }
        }
    }
}

