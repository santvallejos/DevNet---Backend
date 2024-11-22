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
using System.Security.Claims;
using DevNet_BusinessLayer.Services;
using DevNet_BusinessLayer.DTOs;

namespace DevNet_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly DevnetDBContext _context;
        private readonly PostService _postService;

        public PostsController(DevnetDBContext context,PostService postService)
        {
            _context = context;
            _postService = postService;
        }

        // GET: api/Posts/5
        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<ActionResult<Post>> GetUserPosts(Guid userId)
        {
            var result = await _postService.GetUserPostsAsync(userId);

            if (result != null) return Ok(result);
            return BadRequest("No se han podido obtener las publicaciones de este usuario.");
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Post>> GetPost(Guid id)
        {
            return await _postService.GetPostAsync(id);
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditPost(Guid id, [FromBody] EditPostDto postDto)
        {
            var result = await _postService.EditPostAsync(id, postDto);

            if (result) return Ok("Publicación editada con éxito.");
            return BadRequest("No se ha podido editar la publicación.");
        }

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Post>> NewPost([FromBody] NewPostDto postDto)
        {
            var result = await _postService.NewPostAsync(postDto);

            if (result) return Ok("Publicación creada con éxito.");
            return BadRequest("No se ha podido crear la publicación.");
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(Guid id, [FromBody] DeletePostDto deleteDto)
        {
            var result = await _postService.DeletePostAsync(id,deleteDto);

            if (result) return Ok("Publicación eliminada con éxito.");
            return BadRequest("No se ha podido eliminar la publicación.");
        }

        private bool PostExists(Guid id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
