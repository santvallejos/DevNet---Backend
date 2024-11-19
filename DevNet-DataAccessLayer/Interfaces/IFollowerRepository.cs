using DevNet_DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Interfaces
{
    public interface IFollowerRepository : IRepository<Follower>
    {
        Task<IEnumerable<Follower>> GetUserFollowers(Guid userId);

        Task<IEnumerable<Follower>> GetUserFollows(Guid userId);
    }
}
