using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Services;
using System.Security.Claims;

namespace ProjetNET.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class UserTaskController : Controller
    {
        public readonly TaskService _service;
        public UserTaskController(TaskService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult Index()
        {
            // Get the user's ID from the claims in the JWT token
            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(jwtToken);
            }

            // Now you have the user ID, and you can use it to fetch user data and historique
            var user = _service._db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }
            var tasks = _service.GetCurrentTasksByUserId(userId);
            return Ok(tasks);
        }
    }
}
