using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Interfaces;
using DevNet_DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Repositories
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        public ChatRepository(DevnetDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Chat>> GetUserChats(Guid userId)
        {
            return await _context.Chats.Where(u => u.SenderId == userId || u.ReceiverId == userId).ToListAsync();
        }


        public async Task<Chat> GetByIdAsync(Guid id)
        {
            return await _context.Chats.FindAsync(id);
        }
    }
}
