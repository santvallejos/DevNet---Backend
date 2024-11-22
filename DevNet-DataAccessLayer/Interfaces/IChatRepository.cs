using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevNet_DataAccessLayer.Models;

namespace DevNet_DataAccessLayer.Interfaces
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<IEnumerable<Chat>> GetUserChats(Guid userId);
        Task<Chat> GetByIdAsync(Guid id);
    }
}
