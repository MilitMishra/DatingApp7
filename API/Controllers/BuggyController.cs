

using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
            
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecrets(){
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetUserNotFound(){
            var thing = _context.Users.Find(-1);

            if(thing==null) return NotFound();

            return thing;
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError(){
            var thing = _context.Users.Find(-1);

            var thingsToReturn = thing.ToString();

            return thingsToReturn;
        }


        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest(){
            return BadRequest("this is a bad request!");
        }
    }
}