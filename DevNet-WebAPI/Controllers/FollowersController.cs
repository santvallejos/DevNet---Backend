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
using DevNet_DataAccessLayer.Interfaces;
using DevNet_BusinessLayer.Services;
using DevNet_BusinessLayer.DTOs;

namespace DevNet_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowersController : ControllerBase
    {
        private readonly DevnetDBContext _context;
        private readonly FollowerService _followerService;

        public FollowersController(DevnetDBContext context, FollowerService followerService)
        {
            _context = context;
            _followerService = followerService;

        }

        // GET: api/Followers/follows/5
        [HttpGet("follows/{id}")]
        [Authorize]
        public async Task<ActionResult<Follower>> GetFollows(Guid id)
        {
            var result = await _followerService.GetUserFollowsAsync(id);
            if (result != null) return Ok(result);
            return BadRequest();
        }

        // GET: api/Followers/followers/5
        [HttpGet("followers/{id}")]
        [Authorize]
        public async Task<ActionResult<Follower>> GetFollowers(Guid id)
        {
            var result = await _followerService.GetUserFollowersAsync(id);
            if (result != null) return Ok(result);
            return BadRequest();
        }
        
        // POST: api/Followers
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Follower>> AddFollower([FromBody] AddFollowerDto followerDto)
        {
            var result = await _followerService.AddFollowAsync(followerDto);
            if (result) return Ok("Se ha seguido al usuario con exito.");
            return BadRequest("No se ha podido seguir al usuario.");
        }

        // DELETE: api/unfollow
        [HttpDelete("unfollow")]
        [Authorize]
        public async Task<IActionResult> Unfollow(UnfollowDto unfollowDto)
        {
            var result = await _followerService.UnfollowAsync(unfollowDto);
            if (result) return Ok("Se ha dejado de seguir al usuario con exito.");
            return BadRequest("No se ha podido dejar de seguir al usuario.");
        }

        private bool FollowerExists(Guid id)
        {
            return _context.Followers.Any(e => e.FollowerId == id);
        }
    }
}
