using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Models;
using DevNet_BusinessLayer.DTOs;
using DevNet_BusinessLayer.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DevNet_BusinessLayer.Services;

namespace DevNet_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly DevnetDBContext _context;
        private readonly ChatService _chatService;

        public ChatsController(DevnetDBContext context,ChatService chatService)
        {
            _context = context;
            _chatService = chatService;
        }

        // GET: api/Chats
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats()
        {
            var chats = await _chatService.GetChatsAsync();
            return Ok(chats);
        }

        // GET: api/Chats/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Chat>> GetChat(Guid id, [FromBody] GetChatDto chatDto)
        {
            var chat = await _chatService.GetChatAsync(id, chatDto);
            return Ok(chat);
        }


        // POST: api/Chats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IActionResult>> SendChat([FromBody] SendChatDto chatDto)
        {
            var state = await _chatService.SendChat(chatDto);

            if (state) return Ok(state);
            return BadRequest(state);
        }

        // DELETE: api/Chats/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteChat(Guid id, [FromBody] DeleteChatDto chatDto)
        {
            var state = await _chatService.DeleteChat(id, chatDto);
            if (state) return Ok();
            return BadRequest();
        }

        private bool ChatExists(Guid id)
        {
            return _context.Chats.Any(e => e.Id == id);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> CheckChatAsSeen(Guid id)
        {
            var state = await _chatService.CheckChatAsSeen(id);
            if (state) return Ok();
            return BadRequest();
        }
    }
}
