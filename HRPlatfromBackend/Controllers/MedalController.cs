using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjetNET.Models;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;

namespace ProjetNET.Controllers
{
    [Route("medals")]
    [ApiController]
    [Authorize]
    public class MedalController : Controller
    {
        AppDbContext _db;
        public MedalController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet("user")]
        [Authorize(Roles = "User")]
        public ActionResult<object> GetUserWarnings()
        {
            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(jwtToken);
            }

            var userMedals = _db.Medals.Where(w => w.UserId.Equals(userId)).ToList();
            return Ok(userMedals);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<object> GetAllWarnings()
        {
            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(jwtToken);
            }

            var userMedals = _db.Medals.Where(w => w.UserId.Equals(userId)).ToList();
            return Ok(userMedals);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<object> AddUserMedal([FromBody] Medal m)
        {
            if (m == null)
            {
                return BadRequest();

            }
            m.Id = Guid.NewGuid();
            m.Date = DateTime.Now;
            
            _db.Medals.Add(m);
            _db.SaveChanges();

            return Ok("medal added successfully");
        }

        [HttpPatch("{medalId}")]
        public IActionResult AddContest(Guid medalId, [FromBody] JsonPatchDocument<Medal> patch)
        {
            if ((medalId == Guid.Empty) || (patch == null))
            {
                return BadRequest();
            }
            var entity = _db.Medals.FirstOrDefault(u => u.Id == medalId); ;

            if (entity == null)
            {
                return NotFound();
            }

            patch.ApplyTo(entity, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.SaveChanges();

            return NoContent();
        }



        [HttpDelete("{medalId:Guid}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMedal(Guid medalId)
        {
            if (medalId == Guid.Empty)
            {
                return BadRequest();
            }
            var b = _db.Medals.FirstOrDefault(u => u.Id == medalId);
            if (b == null)
            {
                return NotFound();
            }

            _db.Medals.Remove(b);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
