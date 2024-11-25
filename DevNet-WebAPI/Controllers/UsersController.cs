using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevNet_BusinessLayer.Services;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Models;
using DevNet_WebAPI.Infrastructure.DTO;
using DevNet_WebAPI.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevNet_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DevnetDBContext _context;
        private readonly UserAccountService _userAccountService;
        private readonly FileHandlingService _fileHandlingService;
        private readonly UserService _userService;

        public UsersController(DevnetDBContext context, UserAccountService userAccountService, FileHandlingService fileHandlingService, UserService userService)
        {
            _context = context;
            _userAccountService = userAccountService;
            _fileHandlingService = fileHandlingService;
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUsers()
        {
            var result = await _userService.GetUsersAsync();
            if (result != null) return Ok(result);
            return BadRequest();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GetUserDto>> GetUser(Guid id)
        {
            return await _userService.GetUserAsync(id);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditUser(Guid id, [FromBody] EditUserDto userDto)
        {
            var state = await _userService.EditUserAsync(id, userDto);
            if (state) return Ok();
            return BadRequest();
        }

        // POST: api/Users
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDto userDto)
        {
            try
            {
                // Validar que los datos esenciales no estén vacíos
                if (string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
                {
                    return BadRequest("Username, email, and password cannot be empty.");
                }

                // Validación de la imagen en Base64 si existe
                string imageUrl = null;
                if (!string.IsNullOrEmpty(userDto.ProfileImageUrl))
                {
                    // Validar que la cadena Base64 sea correcta
                    try
                    {
                        imageUrl = await _fileHandlingService.SaveProfileImageAsync(userDto.ProfileImageUrl);
                    }
                    catch (FormatException)
                    {
                        return BadRequest("Invalid Base64 image format.");
                    }
                }

                User user = new User
                {
                    Id = Guid.NewGuid(),
                    RoleId = userDto.RoleId,
                    Name = userDto.Name,
                    LastName = userDto.LastName,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Password = userDto.Password,
                    ProfileImageUrl = imageUrl, // Usar la URL generada
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear usuario: {ex.Message}");
            }
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

            try
            {
                // Eliminar la imagen si existe
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    _fileHandlingService.DeleteProfileImage(user.ProfileImageUrl);
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar usuario: {ex.Message}");
            }
        }

        [HttpPut("{id}/username")]
        [Authorize]
        public async Task<IActionResult> UpdateUsername(Guid id, [FromBody] string newUsername)
        {
            if (string.IsNullOrEmpty(newUsername))
            {
                return BadRequest("Username cannot be empty.");
            }

            var user = await _userAccountService.UpdateUsernameAsync(id, newUsername);
            return user == null ? NotFound() : NoContent();
        }

        [HttpPut("{id}/email")]
        [Authorize]
        public async Task<IActionResult> UpdateEmail(Guid id, [FromBody] string newEmail)
        {
            if (string.IsNullOrEmpty(newEmail))
            {
                return BadRequest("Email cannot be empty.");
            }

            var user = await _userAccountService.UpdateEmailAsync(id, newEmail);
            return user == null ? NotFound() : NoContent();
        }

        [HttpPut("{id}/password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                return BadRequest("Password cannot be empty.");
            }

            var user = await _userAccountService.UpdatePasswordAsync(id, newPassword);
            return user == null ? NotFound() : NoContent();
        }

        [HttpPut("{id}/role")]
        [Authorize]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] Guid newRoleId)
        {
            var result = await _userAccountService.UpdateRoleAsync(id, newRoleId);

            if (result) return Ok("Rol cambiado exitosamente.");
            return BadRequest("No se ha podido cambiar el rol de este usuario.");
        }
    }
}
