using DevNet_DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetUserCommentaries(Guid userId);
        Task<Comment> GetByIdAsync(Guid id);
    }
}
