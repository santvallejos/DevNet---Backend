using System;
using System.Threading.Tasks;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DevNet_WebAPI.Services
{
    public class UserAccountService
    {
        private readonly DevnetDBContext _context;

        public UserAccountService(DevnetDBContext context)
        {
            _context = context;
        }

        public async Task<User> UpdateUsernameAsync(Guid userId, string newUsername)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            user.Username = newUsername;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateEmailAsync(Guid userId, string newEmail)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            user.Email = newEmail;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdatePasswordAsync(Guid userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            // Asegúrate de aplicar hashing a la contraseña en la implementación final
            user.Password = newPassword;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateRoleAsync(Guid userId, Guid newRoleId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            user.RoleId = newRoleId;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
