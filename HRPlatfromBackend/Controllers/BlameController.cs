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
    [Route("blames")]
    [ApiController]
    [Authorize]
    public class BlameController : Controller
    {
        AppDbContext _db;
        public BlameController(AppDbContext db)
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

            var userBlames = _db.Blames.Where(w => w.UserId.Equals(userId)).ToList();
            return Ok(userBlames);
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

            var userBlames = _db.Blames.Where(w => w.UserId.Equals(userId)).ToList();
            return Ok(userBlames);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<object> AddUserBlame([FromBody] Blame b)
        {
            if (b == null)
            {
                return BadRequest();

            }
           
            var blame = new Blame
            {
                Id = Guid.NewGuid(),
                DateCreation = DateTime.Now,
                Object = b.Object,
                Name = b.Name,
                UserId=b.UserId,
            };
            _db.Blames.Add(blame);
            _db.SaveChanges();

            return Ok(blame);
        }

        [HttpPatch("{blameId}")]
        public IActionResult AddContest(Guid blameId, [FromBody] JsonPatchDocument<Blame> patch)
        {
            if ((blameId == Guid.Empty) || (patch == null))
            {
                return BadRequest();
            }
            var entity = _db.Blames.FirstOrDefault(u => u.Id == blameId); ;

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



        [HttpDelete("{blameId:Guid}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteBlame(Guid blameId)
        {
            if (blameId == Guid.Empty)
            {
                return BadRequest();
            }
            var b = _db.Blames.FirstOrDefault(u => u.Id == blameId);
            if (b == null)
            {
                return NotFound();
            }

                _db.Blames.Remove(b);
                _db.SaveChanges();
            
            return NoContent();
        }
    }
}
