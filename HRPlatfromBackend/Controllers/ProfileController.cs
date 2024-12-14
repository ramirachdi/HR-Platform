using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Bogus.DataSets;

namespace ProjetNET.Controllers
{
    [Route("api/Profile")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {

        AppDbContext _db;
        public ProfileController(AppDbContext db)
        {
            _db = db;
        }





        [HttpGet("/Profile")]
        public ActionResult<object> GetUserAndHistorique()
        {
            // Get the user's ID from the claims in the JWT token
            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(jwtToken);
            }

            // Now you have the user ID, and you can use it to fetch user data and historique
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var currentDate = DateTime.Now;

            var histoPresencesData = _db.HistoriquePresences
                .Where(h => h.UserId == userId && h.Meeting.Date < currentDate)
                .Select(h => new
                {
                    Name = h.Meeting.Name,
                    Description = h.Meeting.Description,
                    Date = h.Meeting.Date,
                    Validation = h.Presence,
                    Cause = h.Cause,
                    EventType = "Meeting"
                })
                .ToList();

            var histoTasksData = _db.ValidationTasks
                .Where(h => h.UserId == userId && h.Task.DeadLine < currentDate)
                .Select(h => new
                {
                    Name = h.Task.Name,
                    Description = h.Task.Description,
                    Date = h.Task.DeadLine,
                    Validation = h.Validation,
                    Cause = h.Cause,
                    EventType = "Task"
                })
                .ToList();

            var combinedData = histoPresencesData
                .Concat(histoTasksData)
                .OrderBy(e => e.Date)
                .ToList();

            var profileData = new
            {
                UserData = user,
                histoData = combinedData,
            };

            return Ok(profileData);
        }




    }
}




