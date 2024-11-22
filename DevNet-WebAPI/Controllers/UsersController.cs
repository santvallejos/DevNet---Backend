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
using Microsoft.AspNetCore.Authorization;
using DevNet_BusinessLayer.Services;

namespace DevNet_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DevnetDBContext _context;
        private readonly UserAccountService _userAccountService;
        private readonly UserService _userService;

        public UsersController(DevnetDBContext context,UserAccountService userAccount, UserService userService)
        {
            _context = context;
            _userAccountService = userAccount;
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditUser(Guid id, [FromBody] EditUserDto userDto)
        {
            var state = await _userService.EditUserAsync(id, userDto);
            if (state) return Ok();
            return BadRequest();
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
            var result = await _userAccountService.UpdateRoleAsync(id, newRoleId);

            if (result) return Ok("Rol cambiado exitosamente.");
            return BadRequest("No se ha podido cambiar el rol de este usuario.");
        }
    }
}
