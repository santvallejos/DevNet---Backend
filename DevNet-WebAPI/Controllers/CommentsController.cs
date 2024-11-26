using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DevNet_BusinessLayer.Services;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Models;
using DevNet_WebAPI.Infrastructure.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace DevNet_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly DevnetDBContext _context;
        private readonly CommentService _commentService;

        public CommentsController(DevnetDBContext context, CommentService commentService)
        {
            _context = context;
            _commentService = commentService;
        }

        // GET: api/Comments
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            var result = await _commentService.GetCommentsAsync();
            if (result != null) return Ok(result);

            return BadRequest("No se pudieron obtener los comentarios.");
            
        }
        
        // GET: api/Comments/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Comment>> GetComment(Guid id)
        {
            var result = await _commentService.GetCommentAsync(id);
            if (result != null) return Ok(result);

            return BadRequest("No se pudo obtener el comentario.");
        }

        // PUT: api/Comments/edit/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("edit/{id}")]
        [Authorize]
        public async Task<IActionResult> EditComment(Guid id, [FromBody] EditCommentDto commentDto)
        {
            var result = await _commentService.EditCommentAsync(id, commentDto);

            if (result) return Ok("Comentario modificado.");
            return BadRequest("No se pudo editar el comentario.");
        }

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Comment>> PostComment([FromBody] PostCommentDto commentDto)
        {
            var result = await _commentService.PostCommentAsync(commentDto);

            if (result) return Ok("Comentario publicado.");
            return BadRequest("No se pudo publicar el comentario.");
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(Guid id, [FromBody] DeleteCommentDto deleteDto)
        {
            var result = await _commentService.DeleteCommentAsync(id, deleteDto);

            if (result) return Ok("Comentario eliminado.");
            return BadRequest("No se pudo eliminar el comentario.");
        }

        private bool CommentExists(Guid id)
        {
            return _commentService.CommentExist(id);
        }
    }
}
