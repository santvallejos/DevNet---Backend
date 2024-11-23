using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DevNet_BusinessLayer.Interfaces;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Models;
using DevNet_BusinessLayer.DTOs;
using DevNet_DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevNet_BusinessLayer.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly DevnetDBContext _context;

        public ChatService(IChatRepository chatRepository, DevnetDBContext context)
        {
            _chatRepository = chatRepository;
            _context = context;
        }

        public async Task<IEnumerable<Chat>> GetChatsAsync()
        {
            return await _chatRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Chat>> GetUserChatsAsync(Guid userId)
        {
            return await _chatRepository.GetUserChats(userId);
        }

        public async Task<IEnumerable<Chat>> GetChatsBetweenAsync(Guid userId, Guid otherUserId)
        {
            var chats = await _chatRepository.GetUserChats(userId);

            var filteredChat = chats.Where(
                c =>
                c.SenderId == otherUserId
                ||
                c.ReceiverId == otherUserId
                );

            return filteredChat;
        }

        public async Task<Chat> GetChatAsync(Guid id, GetChatDto chatDto)
        {
            return await _chatRepository.GetByIdAsync(id);
        }

        public async Task<bool> SendChat(SendChatDto chatDto)
        {
            Chat newChat = new Chat
            {
                Id = Guid.NewGuid(),
                SenderId = chatDto.SenderId,
                ReceiverId = chatDto.ReceiverId,
                IsRead = false,
                SentAt = DateTime.UtcNow,
                Text = chatDto.Text
            };

            var result = await _chatRepository.AddAsync(newChat);
            
            if (result)await _chatRepository.SaveChangesAsync();
            return result;
        }

        public async Task<bool> DeleteChat(Guid id, DeleteChatDto deleteDto)
        {
            var chatToDelete = await _chatRepository.GetByIdAsync(id);

            var result = await _chatRepository.DeleteAsync(chatToDelete);

            if (result) await _chatRepository.SaveChangesAsync();
            return result;
        }

        public async Task<bool> CheckChatAsSeen(Guid id)
        {
            var chat = await _chatRepository.GetByIdAsync(id);

            chat.IsRead = true;

            var result = await _chatRepository.UpdateAsync(chat);

            if (result) await _chatRepository.SaveChangesAsync();
            return result;
        }
    }
}
