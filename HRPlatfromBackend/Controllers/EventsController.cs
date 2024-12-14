using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ProjetNET.Controllers
{
    [Route("api/Events")]
    [ApiController]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public EventsController(AppDbContext db)
        {
            _db = db;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]

        [HttpGet(Name = "GetEvents")]
        public ActionResult<List<Event>> GetAllEvents()
        {
            List<Event> e = _db.Events.ToList();
            return Ok(e);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [HttpGet("{id:Guid}", Name = "GetEvent")]
        public ActionResult<Event> GetEvent(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            var e = _db.Events.FirstOrDefault(u => u.Id == id);
            if (e == null)
            {
                return NotFound();
            }
            return Ok(e);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(Name ="AddEvent")]
        [Authorize(Roles = "Admin")]

        public ActionResult<Event> PostEvent([FromBody] Event e)
        {
            if (e == null)
            {
                return BadRequest();

            }
            e.Id = Guid.NewGuid();
            _db.Events.Add(e);
            _db.SaveChanges();
            return CreatedAtRoute("GetEvent", new {id =e.Id},e);

        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        [HttpDelete("{id:Guid}", Name ="DeleteEvent")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteEvent(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            var e = _db.Events.FirstOrDefault(u => u.Id == id);
            if (e == null)
            {
                return NotFound();
            }
            _db.Events.Remove(e);   
            _db.SaveChanges();
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [HttpPut("{id:Guid}", Name ="EditEvent")]
        [Authorize(Roles = "Admin")]

        public IActionResult EditEvent(Guid id, [FromBody] Event e)
        {
            if ((id != e.Id) || (e == null))
            {
                return BadRequest();
            }

            var newEvent = _db.Events.FirstOrDefault(u => u.Id == id);

            if (newEvent == null)
            {
                return NotFound();
            }

            newEvent.Name = e.Name;
            newEvent.Location=e.Location;
            newEvent.Description= e.Description;    
            newEvent.Date = e.Date;

            _db.SaveChanges();

            return NoContent();
        }


        [HttpPatch("{id:Guid}", Name = "EditPartialEvent")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditPartialEvent(Guid id,JsonPatchDocument<Event>patch)
        {
            if ((id ==Guid.Empty) || (patch == null))
            {
                return BadRequest();
            }

            var newEvent = _db.Events.FirstOrDefault(u => u.Id == id);

            if (newEvent == null)
            {
                return NotFound();
            }
            patch.ApplyTo(newEvent,ModelState);
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            _db.SaveChanges();
            return NoContent();
        }


    }
}
