using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;
using ProjetNET.Services;

namespace ProjetNET.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class ValidationTaskController : Controller
    {
        private readonly ValidationTaskService _service;
        public ValidationTaskController(ValidationTaskService service)
        {
            _service= service;
        }
        
        [HttpPost]
        public IActionResult Validate([FromBody] ValidationTaskForm validationTaskForm) { 
            if (validationTaskForm == null) { return BadRequest("formulaire vide"); }
            _service.ValidateTask(validationTaskForm);
            return Ok ();
        }
        
    }
}
