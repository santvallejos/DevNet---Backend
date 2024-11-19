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
    public class FollowerRepository : Repository<Follower>, IFollowerRepository
    {
        public FollowerRepository(DevnetDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Follower>> GetUserFollows(Guid userId)
        {
            return await _context.Followers.Where(f => f.FollowerId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Follower>> GetUserFollowers(Guid userId)
        {
            return await _context.Followers.Where(f => f.FollowedId == userId).ToListAsync();
        }
        
    }
}
