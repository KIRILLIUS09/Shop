using Microsoft.AspNetCore.Mvc;
using BisLogic.Services;
using orm;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected readonly AppDbContext _context;

        public BaseController(AppDbContext context)
        {
            _context = context;
        }

        protected ActionResult HandleError(Exception ex, string message = "Произошла ошибка")
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { error = message });
        }
    }
}
