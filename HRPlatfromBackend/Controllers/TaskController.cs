using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;
using ProjetNET.Services;

namespace ProjetNET.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles ="Admin")]
    public class TaskController : Controller
    {
        public readonly TaskService _service;
        public TaskController(TaskService service)
        {
            _service = service;
        }
        
        [HttpPost("create")]
        public IActionResult Create([FromBody] Models.Task taskform) {
            if (taskform==null)
            {
                return BadRequest("form vide");
            }
            if(taskform.Users==null || taskform.Users.Count()==0 || taskform.Name == ""|| taskform.Description=="")
            {
                return BadRequest("champ(s) vide");
            }
           _service.CreateTask(taskform);
            return Ok();
        }
        [HttpGet]
        public IActionResult GetTasks()

        {
            Console.WriteLine("getting all tasks ");
            var tasks = _service.GetCurrentTasks();
            return Ok(tasks);
        }

    }
}
