using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Models;
using ProjetNET.Services;

namespace ProjetNET.Controllers
{
    [Route("[controller]")]
   // [Authorize]
    public class AnonymBoxCommentController : Controller
    { public readonly AnonymBoxCommentService _service;
        public AnonymBoxCommentController(AnonymBoxCommentService service)
        {
            _service= service;
        }

        [HttpGet("index")]
        public ActionResult<List<AnonymBoxComment>> Index()
        {   List<AnonymBoxComment> comments=_service.display_comments();
            return Ok(comments);
        }
        [HttpPost("create")]
        public ActionResult Create([FromBody] AnonymBoxCommentForm? formData)
        {   

            if(formData == null | formData.Contenu=="")
            {
                return BadRequest("formulaire vide");
            }
            string? contenu = formData.Contenu;
            
            _service.create_comment(contenu);
            return Ok("commentaire ajouté");
        }

    }
}
