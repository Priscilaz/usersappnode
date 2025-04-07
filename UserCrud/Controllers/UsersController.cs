using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserCrud.Models;
using System.Linq;
using System.Threading.Tasks;
using UserCrud.Data;

namespace UserCrud.Controllers
{
    [Route("api/usersapp")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // Read
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var users = await _context.Users
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone
                })
                .ToListAsync();

            return Ok(users);
        }

        // Create
        [HttpPost("new")]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User is null");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(List), new { id = user.Id }, user);
        }

        // Update
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            // existingUser.Password = user.Password; // No actualizar la contraseña aquí

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
