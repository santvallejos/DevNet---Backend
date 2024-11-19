using System;
using System.Threading.Tasks;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Interfaces;
using DevNet_DataAccessLayer.Models;
using DevNet_WebAPI.Infrastructure.DTO;
using DevNet_BusinessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace DevNet_BusinessLayer.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly DevnetDBContext _context;

        public UserAccountService(DevnetDBContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto userDto)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            User user = new User
            {
                Id = Guid.NewGuid(),
                RoleId = userDto.RoleId,
                Name = userDto.Name,
                LastName = userDto.LastName,
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password,
                ProfileImageUrl = userDto.ProfileImageUrl,
                CreatedAt = DateTime.Now
            };
           
            user.Password = passwordHasher.HashPassword(user,userDto.Password);

            var result = await _userRepository.AddAsync(user);

            return result;
        }

        public async Task<bool> UpdateUsernameAsync(Guid userId, string newUsername)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.Username = newUsername;

            var result = await _userRepository.UpdateAsync(user);

            return result;
        }

        public async Task<bool> UpdateEmailAsync(Guid userId, string newEmail)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.Email = newEmail;

            var result = await _userRepository.UpdateAsync(user);

            return result;
        }

        public async Task<bool> UpdatePasswordAsync(Guid userId, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            var passwordHasher = new PasswordHasher<User>();

            user.Password = passwordHasher.HashPassword(user,newPassword);

            var result = await _userRepository.UpdateAsync(user);

            return result;
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

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var userToDelete = await _userRepository.GetByIdAsync(id);
            return await _userRepository.DeleteAsync(userToDelete);
        }
    }
}
