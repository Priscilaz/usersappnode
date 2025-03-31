using Microsoft.AspNetCore.Mvc;
using UserCrud.Models;

namespace UserCrud.Controllers
{
    [Route("api/usersapp")]
    [ApiController]
    public class UsersController : Controller
    {
        private static List<User> _users = new List<User>();

        //Read
        [HttpGet("list")]
        public IActionResult List()
        {
            return Ok(_users);  
        }

        //Create
        [HttpPost("new")]
        public IActionResult Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User is null");
            }
            user.Id = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;
            _users.Add(user);
            return CreatedAtAction(nameof(List), new { id = user.Id }, user); 
        }
                
        //Update   
        [HttpPut("update/{id}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return NotFound(); 
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.Phone = user.Phone;
            return NoContent(); 
        }

        //Delete
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(); 
            }

            _users.Remove(user);
            return NoContent(); 
        }

    }
}
