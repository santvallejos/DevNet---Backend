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
using Microsoft.Extensions.Hosting;

namespace DevNet_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly DevnetDBContext _context;

        public CommentsController(DevnetDBContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }
        
        // GET: api/Comments/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Comment>> GetComment(Guid id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditComment(Guid id, [FromBody] EditCommentDto commentDto)
        {
            if (id != commentDto.Id)
            {
                return BadRequest();
            }

            // Obtener el UserId del token JWT
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar que el UserId del token coincida con el UserId del postDto
            if (userIdFromToken != commentDto.UserId.ToString())
            {
                return Unauthorized("El usuario que intenta editar el comentario no coincide con el que ha iniciado sesion.");
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            if (comment.UserId != commentDto.UserId)
            {
                return Unauthorized("No tienes permiso de editar este comentario.");
            }

            comment.Text = commentDto.Text;
            


            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Comment>> PostComment([FromBody] PostCommentDto commentDto)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar que el UserId del token coincida con el UserId del postDto
            if (userIdFromToken != commentDto.UserId.ToString())
            {
                return Unauthorized("El usuario que intenta dejar un nuevo comentario no coincide con el que ha iniciado sesion.");
            }

            Comment comment = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = commentDto.PostId,
                UserId = commentDto.UserId,
                Text = commentDto.Text,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(Guid id, [FromBody] DeleteCommentDto deleteDto)
        {
            if (id != deleteDto.Id)
            {
                return BadRequest();
            }
            
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar que el UserId del token coincida con el UserId del postDto
            if (userIdFromToken != deleteDto.UserId.ToString())
            {
                return Unauthorized("El usuario que intenta eliminar el comentario no coincide con el que ha iniciado sesion.");
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            if (comment.UserId != deleteDto.UserId)
            {
                return Unauthorized("No tienes permiso de borrar este comentario.");
            }


            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(Guid id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
