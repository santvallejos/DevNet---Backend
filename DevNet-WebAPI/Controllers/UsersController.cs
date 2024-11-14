using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Models;
using DevNet_WebAPI.Infrastructure.DTO;

namespace DevNet_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DevnetDBContext _context;

        public UsersController(DevnetDBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            // Convertir cada usuario a UserReadDto
            var userDtos = users.Select(user => new GetUserDto
            {
                Id = user.Id,
                RoleId = user.RoleId,
                Name = user.Name,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                CreatedAt = user.CreatedAt
            }).ToList();

            return userDtos;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDto>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            GetUserDto userDto = new GetUserDto
            {
                Id = user.Id,
                RoleId = user.RoleId,
                Name = user.Name,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                CreatedAt = user.CreatedAt
            };

            return userDto;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(Guid id, EditUserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = userDto.Name;
            user.LastName = userDto.LastName;
            user.ProfileImageUrl = userDto.ProfileImageUrl;

            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(CreateUserDto userDto)
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                RoleId = userDto.RoleId,
                Name = userDto.Name,
                LastName = userDto.LastName,
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password,
                ProfileImageUrl = userDto.ProfileImageUrl,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
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

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
