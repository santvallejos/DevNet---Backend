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
    public class CommentRepository: Repository<Comment>, ICommentRepository
    {
        public CommentRepository(DevnetDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Comment>> GetUserCommentaries(Guid userId)
        {
            return await _context.Comments.Where(c => c.UserId == userId).ToListAsync();
        }


        public async Task<Comment> GetByIdAsync(Guid id)
        {
            return await _context.Comments.FindAsync(id);
        }
    }
}
