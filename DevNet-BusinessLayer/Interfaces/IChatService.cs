using DevNet_BusinessLayer.DTOs;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Models;

namespace DevNet_BusinessLayer.Interfaces
{
    public interface IChatService
    {
        Task<Chat> GetChatAsync(Guid id, GetChatDto chatDto);
        Task<IEnumerable<Chat>> GetChatsAsync();
    }
}