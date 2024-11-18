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
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DevNet_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly DevnetDBContext _context;

        public ChatsController(DevnetDBContext context)
        {
            _context = context;
        }

        // GET: api/Chats
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats([FromQuery] GetChatsDto chatDto)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar si el usuario está autenticado
            if (userIdFromToken == null)
            {
                return Unauthorized("No estás autenticado.");
            }

            // Verificar que el UserId del token coincida con el UserId del chatDto
            if (userIdFromToken != chatDto.UserId.ToString())
            {
                return Unauthorized("No tienes permiso para acceder a estos mensajes.");
            }

            // Convertir el UserId del token a Guid
            Guid userId = Guid.Parse(userIdFromToken);

            // Filtrar los chats entre el usuario autenticado y el usuario relacionado
            var chatsQuery = _context.Chats
                .Where(c =>
                    (c.SenderId == userId && c.ReceiverId == chatDto.RelatedUserId) ||
                    (c.SenderId == chatDto.RelatedUserId && c.ReceiverId == userId))
                .OrderByDescending(c => c.SentAt); // Ordenar por la fecha de envío, de más reciente a más antiguo

            // Obtener el total de chats que cumplen con los filtros
            var totalChats = await chatsQuery.CountAsync();

            // Obtener los chats con paginación
            var chats = await chatsQuery
                .Skip((chatDto.Page - 1) * chatDto.PageSize)  // Calcular el número de registros a omitir
                .Take(chatDto.PageSize)                       // Tomar el número de registros de acuerdo al tamaño de la página
                .ToListAsync();

            // Crear el objeto de respuesta con la paginación
            var pagedResult = new
            {
                TotalChats = totalChats,
                Page = chatDto.Page,
                PageSize = chatDto.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalChats / chatDto.PageSize),
                Chats = chats
            };

            return Ok(pagedResult);
        }

        // GET: api/Chats/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Chat>> GetChat(Guid id, [FromBody] GetChatDto chatDto)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar que el UserId del token coincida con el UserId del chatDto
            if (userIdFromToken != chatDto.SenderId.ToString() && userIdFromToken != chatDto.ReceiverId.ToString())
            {
                return Unauthorized("No tienes permiso para leer este mensaje.");
            }

            var chat = await _context.Chats.FindAsync(id);

            if (chat == null)
            {
                return NotFound();
            }

            // Verificar que el SenderId del chat de la BD coincida con el UserId del chatDto
            if (chat.SenderId != chatDto.SenderId)
            {
                return Unauthorized("El emisor del mensaje que solicitaste no coincide con el de la base de datos.");
            }

            // Verificar que el ReceiverId del chat de la BD coincida con el UserId del chatDto
            if (chat.ReceiverId != chatDto.ReceiverId)
            {
                return Unauthorized("El receptor del mensaje que solicitaste no coincide con el de la base de datos.");
            }

            return chat;
        }

        // PUT: api/Chats/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        /* Codigo comentado en caso de que se quiera que sea posible editar mensajes ya enviados
         * 
         * 
        [HttpPut("{id}")]
        public async Task<IActionResult> EditChat(Guid id, Chat chat)
        {
            if (id != chat.Id)
            {
                return BadRequest();
            }

            _context.Entry(chat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatExists(id))
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
        */

        // POST: api/Chats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Chat>> SendChat([FromBody] SendChatDto chatDto)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar que el UserId del token coincida con el UserId del chatDto
            if (userIdFromToken != chatDto.SenderId.ToString())
            {
                return Unauthorized("No tienes permiso para enviar este mensaje.");
            }

            Chat chat = new Chat
            {
                Id = Guid.NewGuid(),
                IsRead = false,
                ReceiverId = chatDto.ReceiverId,
                SenderId = chatDto.SenderId,
                SentAt = DateTime.Now,
                Text = chatDto.Text
            };

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChat", new { id = chat.Id }, chat);
        }

        // DELETE: api/Chats/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteChat(Guid id, [FromBody] DeleteChatDto chatDto)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar que el UserId del token coincida con el UserId del chatDto
            if (userIdFromToken != chatDto.SenderId.ToString())
            {
                return Unauthorized("No tienes permiso para eliminar este mensaje.");
            }

            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }

            if (chat.SenderId != chatDto.SenderId)
            {
                return Unauthorized("No tienes permiso para eliminar este mensaje.");
            }

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChatExists(Guid id)
        {
            return _context.Chats.Any(e => e.Id == id);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> CheckAsSeenChat(Guid id)
        {
            var chat = _context.Chats.Find(id);

            if (chat == null)
            {
                return NotFound();
            }

            chat.IsRead = true;

            if (id != chat.Id)
            {
                return BadRequest();
            }

            _context.Entry(chat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatExists(id))
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
    }
}
