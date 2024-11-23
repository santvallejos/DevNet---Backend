using System;
using System.Threading.Tasks;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Interfaces;
using DevNet_DataAccessLayer.Models;
using DevNet_WebAPI.Infrastructure.DTO;
using DevNet_BusinessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DevNet_DataAccessLayer.Repositories;


namespace DevNet_BusinessLayer.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly UserRepository _userRepository;
        private readonly DevnetDBContext _context;
        private readonly JwtService _jwtService;

        public UserAccountService(DevnetDBContext context, UserRepository userRepository, JwtService jwtService)
        {
            _context = context;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public string LoginUser(LoginDto loginDto)
        {
            // Buscar al usuario en la base de datos
            var user = _context.Users
                .FirstOrDefault(u => u.Username == loginDto.Username);

            if (user == null)
            {
                return null;
            }

            // Verificar la contraseña
            var passwordHasher = new PasswordHasher<User>();
            if (passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password) != PasswordVerificationResult.Success)
            {
                return null;
            }

            // Obtener el nombre del rol usando RoleId del usuario
            var role = _context.Roles
                .FirstOrDefault(r => r.Id == user.RoleId);

            if (role == null)
            {
                return null;
            }

            // Generar el token para el usuario autenticado
            var token = _jwtService.GenerateToken(user.Username, role.Name);  // Usamos el nombre del rol

            return token;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto userDto)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            User user = new User
            {
                Id = Guid.NewGuid(),

                #warning Reemplazar el Id dentro del "Guid.Parse" con el roleId del rol "user" de la base de datos
                RoleId = (Guid)(userDto.RoleId  != null ? userDto.RoleId : Guid.Parse("75946ec7-2cd0-41f6-9221-498c196a4299")),

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

            if (result) await _userRepository.SaveChangesAsync();
            return result;
        }

        public async Task<bool> UpdateUsernameAsync(Guid userId, string newUsername)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.Username = newUsername;

            var result = await _userRepository.UpdateAsync(user);

            if (result) await _userRepository.SaveChangesAsync();
            return result;
        }

        public async Task<bool> UpdateEmailAsync(Guid userId, string newEmail)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.Email = newEmail;

            var result = await _userRepository.UpdateAsync(user);

            if (result) await _userRepository.SaveChangesAsync();
            return result;
        }

        public async Task<bool> UpdatePasswordAsync(Guid userId, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            var passwordHasher = new PasswordHasher<User>();

            user.Password = passwordHasher.HashPassword(user,newPassword);

            var result = await _userRepository.UpdateAsync(user);

            if (result) await _userRepository.SaveChangesAsync();
            return result;
        }

        public async Task<bool> UpdateRoleAsync(Guid userId, Guid newRoleId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.RoleId = newRoleId;
            
            var result = await _userRepository.UpdateAsync(user);
            

            if (result) await _userRepository.SaveChangesAsync();
            return result;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var userToDelete = await _userRepository.GetByIdAsync(id);
            var result = await _userRepository.DeleteAsync(userToDelete);

            if (result) await _userRepository.SaveChangesAsync();
            return result;
        }
    }
}
