using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Interfaces;
using DevNet_DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DevnetDBContext context) : base(context)
        {
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
