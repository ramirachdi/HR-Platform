using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.IO;
using ProjetNET.Models;
using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetNET.Controllers
{
    [Route("api/meetings")]
    [Authorize]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MeetingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/meetings

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Meeting>> GetMeetings()
        {
            return Ok(_context.Meetings.ToList());
        }

        // GET: api/meetings/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Meeting> GetMeeting(Guid id)
        {
           
            var meeting = _context.Meetings.Find(id);

            if (meeting == null)
            {
                return NotFound();
            }

            return Ok(meeting);
        }
        // GET: api/meetings/upcoming
        [HttpGet("upcoming")]
        [Authorize] // Ensure the user is authenticated
        public IActionResult GetUpcomingMeetings()
        {
            // Get the current date
            DateTime currentDate = DateTime.UtcNow;
            // Get the authenticated user ID
            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];
           
            if (userIdClaim == null )
            {
                return Unauthorized(jwtToken);
            }
            string userIdString = userIdClaim.Value;
            Guid userId = new Guid(userIdString);

            // Get upcoming meetings for the user
            var upcomingMeetings = _context.Meetings
                .Where(m => m.Date.HasValue && m.Date > currentDate && m.Users.Any(u => u.Id == userId))
                .ToList();

            return Ok(upcomingMeetings);
        }
        // GET: api/meetings/mymeetings
        [HttpGet("mymeetings")]
        [Authorize] // Ensure the user is authenticated
        public IActionResult GetUserMeetings()
        {
            // Get the authenticated user ID
            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null)
            {
                return Unauthorized(jwtToken);
            }
            string userIdString = userIdClaim.Value;
            Guid userId = new Guid(userIdString);

            // Get upcoming meetings for the user
            var upcomingMeetings = _context.Meetings
                .Where(m =>  m.Users.Any(u => u.Id == userId))
                .ToList();

            return Ok(upcomingMeetings);
        }

        // POST: api/meetings
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<Meeting> PostMeeting([FromBody] Meeting meeting)
        {
            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null)
            {
                return Unauthorized(jwtToken);
            }
            var isAdmin = HttpContext.User.IsInRole("Admin");

            if (!isAdmin)
            {
                return Forbid();
            }

            if (meeting == null)
            {
                return BadRequest("Meeting data is null");
            }
            if (meeting.Users == null)
            {
                return BadRequest("Meeting  should have members to attend");
            }
            var meetingAttr = new Meeting
            {
                Date = meeting.Date,
                Description = meeting.Description,
                Type = meeting.Type,
                Name = meeting.Name,
                Location = meeting.Location,
            };

            // Add the meeting to the context
            _context.Meetings.Add(meetingAttr);
            //
            // _context.SaveChanges();

            // Create HistoriquePresence instances for each user related to the meeting
            foreach (var userId in meeting.Users)
            {
                var user = _context.Users.FirstOrDefault(c => c.Id == userId.Id);

                if (user != null)
                {
                    // Check if a ValidationTask with the same UserId and TaskId exists
                    var existingHistoriquePresence = _context.HistoriquePresences.FirstOrDefault(vt => vt.UserId == userId.Id && vt.MeetingId == meeting.Id);

                    // If the ValidationTask doesn't already exist, create a new one
                    if (existingHistoriquePresence == null)
                    {
                        var historiquePresence = new HistoriquePresence
                        {
                            UserId = user.Id,
                            MeetingId = meetingAttr.Id,
                            Presence = false // Set to false by default

                        };

                        // Add the HistoriquePresence instance to the context
                        _context.HistoriquePresences.Add(historiquePresence);
                    }
                }
            }

            // Save changes to the database
            _context.SaveChanges();

            return Ok("Added meeting successfully");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/qrcode")]
        public IActionResult GetMeetingQRCode(Guid id)
        {
            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null)
            {
                return Unauthorized(jwtToken);
            }
            var isAdmin = HttpContext.User.IsInRole("Admin");

            if (!isAdmin)
            {
                return Forbid(); 
            }

            var meeting = _context.Meetings.Find(id);

            if (meeting == null)
            {
                return NotFound();
            }

            // Generate QR code for the meeting
            var content = "localhost:3000/scan/"+id;
            var qrCodeImage = GenerateQRCodeImage(content);
            // Return QR code image as a result
            return File(qrCodeImage, "image/png");
        }


        // POST: api/meetings/scan
        [HttpPost("scan")]
        public IActionResult ScanMeetingQRCode([FromBody] QRCodeScanRequest scanRequest)
        {
            string meetingId = scanRequest.meetingId;
            if (meetingId == null)
            {
                return BadRequest("Invalid QR code data");
            }

            Guid meetingIdGuid= new Guid(meetingId);

            var userIdClaim = HttpContext.User.FindFirst("id");
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null)
            {
                return Unauthorized(jwtToken);
            }
            string userIdString = userIdClaim.Value;

            Guid userId = new Guid(userIdString);

            // Check if the meeting exists
            var meeting = _context.Meetings.Find(meetingIdGuid);
            if (meeting == null)
            {
                return NotFound("Meeting not found");
            }

            // Check if the user is already marked as present
            var existingPresence = _context.HistoriquePresences
                .FirstOrDefault(p => p.UserId == userId && p.MeetingId == meetingIdGuid);

            if (existingPresence == null)
            {
                // Create a new entry in HistoriquePresence
                var newPresence = new HistoriquePresence
                {
                    UserId = userId,
                    MeetingId = meetingIdGuid,
                    Presence = true
                };

                _context.HistoriquePresences.Add(newPresence);

            }
            else { existingPresence.Presence = true; }
            _context.SaveChanges();

            return Ok("User presence marked for the meeting");


        }

        // Utility method to generate QR code image
        private byte[] GenerateQRCodeImage(string content)
        {
            byte[] QRCode = new byte[0];
            if (!string.IsNullOrEmpty(content)) {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                BitmapByteQRCode bitmap = new BitmapByteQRCode(qrCodeData);
                QRCode = bitmap.GetGraphic(20); 
            }
            return QRCode;
        }
        // PUT: api/meetings/{id}
        [HttpPut("{id}")]
        [Authorize] // Ensure the user is authenticated
        public IActionResult PutMeeting(Guid id, [FromBody] Meeting updatedMeeting)
        {
            if (updatedMeeting == null)
            {
                return BadRequest("Updated meeting data is null");
            }

            // Check if the provided ID matches the route parameter
            if (id != updatedMeeting.Id)
            {
                return BadRequest("Mismatched meeting ID in the route and request body");
            }

            // Get the authenticated user ID
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var jwtToken = HttpContext.Request.Headers["Authorization"];

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(jwtToken);
            }

            // Check if the meeting exists
            var existingMeeting = _context.Meetings.Find(id);
            if (existingMeeting == null)
            {
                return NotFound("Meeting not found");
            }

            // Update the meeting properties
            existingMeeting.Name = updatedMeeting.Name;
            existingMeeting.Description = updatedMeeting.Description;
            existingMeeting.Type = updatedMeeting.Type;
            existingMeeting.Location = updatedMeeting.Location;
            existingMeeting.Date = updatedMeeting.Date;

            // Save changes to the database
            _context.SaveChanges();

            return Ok(existingMeeting);
        }

        // DELETE: api/meetings/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult<Meeting> DeleteMeeting(Guid id)
        {
            var meeting = _context.Meetings.Find(id);
            if (meeting == null)
            {
                return NotFound();
            }

            _context.Meetings.Remove(meeting);
            _context.SaveChanges();

            return Ok(meeting);
        }
    }

    public class QRCodeScanRequest
    {
        public string meetingId { get; set; }
    }
}
