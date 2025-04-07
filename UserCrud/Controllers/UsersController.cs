using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserCrud.Models;
using System.Linq;
using System.Threading.Tasks;
using UserCrud.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace UserCrud.Controllers
{
    [Route("api/usersapp")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UsersController(AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
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
        public async Task<IActionResult> Create([FromBody] UserDTO userDTO)
        {

            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(userDTO.Password))
            {
                return BadRequest("Datos inválidos o contraseña vacía.");
            }

            var hashedPassword = HashPassword(userDTO.Password);

            var user = new User
            {
                Name = userDTO.Name,
                Phone = userDTO.Phone,
                Email = userDTO.Email,
                PasswordHash = hashedPassword
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var detailedMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, $"Error guardando usuario: {detailedMessage}");
            }


            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(List), new { id = user.Id }, user);
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Update
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDTO userDTO)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Name = userDTO.Name;
            existingUser.Email = userDTO.Email;
            existingUser.Phone = userDTO.Phone;

            if (!string.IsNullOrEmpty(userDTO.Password))
            {
                existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, userDTO.Password);
            }

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
