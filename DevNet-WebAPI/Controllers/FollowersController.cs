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

namespace DevNet_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowersController : ControllerBase
    {
        private readonly DevnetDBContext _context;

        public FollowersController(DevnetDBContext context)
        {
            _context = context;
        }

        // GET: api/Followers
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Follower>>> GetFollowers()
        {
            return await _context.Followers.ToListAsync();
        }

        // GET: api/Followers/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Follower>> GetFollower(Guid id)
        {
            var follower = await _context.Followers.FindAsync(id);

            if (follower == null)
            {
                return NotFound();
            }

            return follower;
        }

        
        // POST: api/Followers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Follower>> AddFollower([FromBody] AddFollowerDto followerDto)
        {
            Follower follower = new Follower
            {
                FollowedAt = DateTime.Now,
                FollowedId = followerDto.FollowedId,
                FollowerId = followerDto.FollowerId
            };
            _context.Followers.Add(follower);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFollower", new { id = follower.FollowerId }, follower);
        }

        // DELETE: api/Followers/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFollower(Guid id)
        {
            var follower = await _context.Followers.FindAsync(id);
            if (follower == null)
            {
                return NotFound();
            }

            _context.Followers.Remove(follower);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FollowerExists(Guid id)
        {
            return _context.Followers.Any(e => e.FollowerId == id);
        }
    }
}
