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
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(DevnetDBContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Post>> GetUserPosts(Guid userId)
        {
            return await _context.Posts
                .Where(post => post.UserId == userId)
                .ToListAsync();
        }

        public async Task<Post> GetPostById(Guid postId)
        {
            return await _context.Posts
                .FirstOrDefaultAsync(post => post.Id == postId);
        }
    }
}
