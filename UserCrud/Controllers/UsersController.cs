using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserCrud.Models;
using System.Linq;
using System.Threading.Tasks;
using UserCrud.Data;
using Microsoft.AspNetCore.Identity;
using System;

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

        // GET: api/usersapp/list
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

        // POST: api/usersapp/new
        [HttpPost("new")]
        public async Task<IActionResult> Create([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(userDTO.Password))
            {
                return BadRequest("Datos inválidos o contraseña vacía.");
            }

            var user = new User
            {
                Name = userDTO.Name,
                Phone = userDTO.Phone,
                Email = userDTO.Email
            };

            // Hashear la contraseña
            user.PasswordHash = _passwordHasher.HashPassword(user, userDTO.Password);

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(List), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                var detailedMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, $"Error guardando usuario: {detailedMessage}");
            }

            
        }

        // PUT: api/usersapp/update/{id}
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

        // DELETE: api/usersapp/delete/{id}
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

        // POST: api/usersapp/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (string.IsNullOrWhiteSpace(loginDTO.Email) || string.IsNullOrWhiteSpace(loginDTO.Password))
            {
                return BadRequest("Email y contraseña son obligatorios.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
            if (user == null)
            {
                return Unauthorized("Credenciales inválidas.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Credenciales inválidas.");
            }

            return Ok(new
            {
                message = "Login exitoso",
                user = new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Phone
                }
            });
        }
    }
}
