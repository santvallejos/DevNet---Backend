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
using DevNet_WebAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevNet_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DevnetDBContext _context;
        private readonly UserAccountService _userAccountService;

        public UsersController(DevnetDBContext context,UserAccountService userAccount)
        {
            _context = context;
            _userAccountService = userAccount;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> EditUser(Guid id, [FromBody] EditUserDto userDto)
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
        [Authorize]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDto userDto)
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
        [Authorize]
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

        [HttpPut("{id}/username")]
        [Authorize]
        public async Task<IActionResult> UpdateUsername(Guid id, [FromBody] string newUsername)
        {
            var user = await _userAccountService.UpdateUsernameAsync(id, newUsername);
            return user == null ? NotFound() : NoContent();
        }

        [HttpPut("{id}/email")]
        [Authorize]
        public async Task<IActionResult> UpdateEmail(Guid id, [FromBody] string newEmail)
        {
            var user = await _userAccountService.UpdateEmailAsync(id, newEmail);
            return user == null ? NotFound() : NoContent();
        }

        [HttpPut("{id}/password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] string newPassword)
        {
            var user = await _userAccountService.UpdatePasswordAsync(id, newPassword);
            return user == null ? NotFound() : NoContent();
        }

        [HttpPut("{id}/role")]
        [Authorize]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] Guid newRoleId)
        {
            var user = await _userAccountService.UpdateRoleAsync(id, newRoleId);
            return user == null ? NotFound() : NoContent();
        }
    }
}
