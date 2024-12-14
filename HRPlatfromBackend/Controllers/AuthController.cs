using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NuGet.Protocol.Plugins;

namespace ProjetNET.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        
        public AuthController(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public ActionResult<User> Register(User request)
        {
            // Check if the request object is null
            if (request == null)
            {
                return BadRequest("Invalid request data");
            }

            // Validate required fields
            if (string.IsNullOrEmpty(request.LastName) ||
                string.IsNullOrEmpty(request.FirstName) ||
                string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.PasswordHash) ||
                string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.Adress) ||
                (request.IsAdmin == null))
            {
                return BadRequest("LastName, FirstName, Email, PasswordHash, Phone Number, Adress, and IsAdmin are required fields");
            }

            // Validate email format
            if (!IsValidEmail(request.Email))
            {
                return BadRequest("Invalid email format");
            }
            if (_db.Users.Any(u => u.Email == request.Email))
            {
                return Conflict("Email already exists");
            }


            // Validate password length or other criteria if needed
            if (request.PasswordHash.Length < 6)
            {
                return BadRequest("Password should be at least 6 characters long");
            }

           

            // Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

            // Create a new user instance
            var user = new User
            {
                LastName = request.LastName,
                FirstName = request.FirstName,
                Email = request.Email,
                PasswordHash = passwordHash,
                PhoneNumber = request.PhoneNumber,
                Adress = request.Adress,
                IsAdmin = request.IsAdmin,
            };

            // Save the user to the database
            _db.Users.Add(user);
            _db.SaveChanges();

            return Ok("User added successfully");
        }
        public class LoginRequestWithRememberMe
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        [HttpPost("login")]
        public ActionResult<string> Login(LoginRequestWithRememberMe loginRequest)
        {
            Console.WriteLine($"RememberMe value received: {loginRequest.RememberMe}");

            // Validate login request
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid login request");
            }

            // Find the user by email
            var user = _db.Users.FirstOrDefault(u => u.Email == loginRequest.Email && u.DeletedAt==null );
             
            // Check if the user exists and verify the password
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password");
            } 
            /*catch (Exception ex)
{
                // Log the exception or handle it appropriately
                // Returning Unauthorized might not be suitable for all cases where an exception occurs
                return StatusCode(500, "An error occurred during authentication");
            }*/
            // Generate JWT token
            var token = GenerateTokenString(user,loginRequest.RememberMe);


            return Ok(new { Message = "Login successful", Token = token });



        }

        [HttpGet("{id:Guid}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public ActionResult<Event> GetUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var e = _db.Users.FirstOrDefault(u => u.Id == id && u.DeletedAt == null);

            if (e == null)
            {
                return NotFound();
            }

            return Ok(e);
        }

        [HttpGet(Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public ActionResult<Event> GetAllUsers()
        {


            var users = _db.Users.Where(u => u.DeletedAt == null).ToList();

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]

        [HttpDelete("{id:Guid}", Name = "Delete")]
        public IActionResult Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            var e = _db.Users.FirstOrDefault(u => u.Id == id && u.DeletedAt==null);
            if (e == null)
            {
                return NotFound();
            }
            e.DeletedAt = DateTime.Now; 
          
            _db.SaveChanges();
            return NoContent();
        }

        [Authorize]
        [HttpGet("isloggedin")]
        public IActionResult IsLoggedIn()
        {
            return Ok("You are logged in");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("isadmin")]
        public IActionResult IsAdmin()
        {
            return Ok("You are an admin");
        }


        [HttpDelete("deletemembers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMembers([FromBody] List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return BadRequest("No user IDs provided for deletion");
            }

            var usersToDelete = _db.Users.Where(u => ids.Contains(u.Id) && u.DeletedAt == null).ToList();
            if (usersToDelete.Count == 0)
            {
                return NotFound("No users found with the provided IDs");
            }
            var currentTime = DateTime.Now;
            foreach (var user in usersToDelete)
            {
                user.DeletedAt = currentTime;
            }

            

            _db.SaveChanges();

            return NoContent();
        }


        // Validate email format
        [NonAction]
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        [NonAction]
        public string GenerateTokenString(User user,bool RememberMe)

        {
            var claims = new List<Claim>
            {
              new Claim("id", user.Id.ToString()),
                    new Claim("role", user.IsAdmin ? "Admin" : "User"),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
            claims: claims,

                expires: RememberMe ? DateTime.UtcNow.AddMonths(12) : DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCred);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
    }






    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
