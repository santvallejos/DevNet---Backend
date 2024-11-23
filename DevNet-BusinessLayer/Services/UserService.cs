using DevNet_BusinessLayer.DTOs;
using DevNet_BusinessLayer.Interfaces;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Interfaces;
using DevNet_DataAccessLayer.Models;
using DevNet_DataAccessLayer.Repositories;
using DevNet_WebAPI.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly DevnetDBContext _context;

        public UserService(IUserRepository userRepository, DevnetDBContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<IEnumerable<GetUserDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            // Convertir cada usuario a UserReadDto
            var userDtos = users.Select(user => new GetUserDto
            {
                Id = user.Id,
                RoleId = user.RoleId,
                Name = user.Name,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                CreatedAt = user.CreatedAt
            }).ToList();

            return userDtos;
        }

       

        public async Task<GetUserDto> GetUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            GetUserDto userDto = new GetUserDto
            {
                Id = user.Id,
                RoleId = user.RoleId,
                Name = user.Name,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                CreatedAt = user.CreatedAt
            };

            return userDto;
        }

        public async Task<bool> EditUserAsync(Guid id, EditUserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            user.Name = userDto.Name;
            user.LastName = userDto.LastName;
            user.ProfileImageUrl = userDto.ProfileImageUrl;

            if (id != user.Id)
            {
                return false;
            }


            try
            {
                var result = await _userRepository.UpdateAsync(user);

                if (result) await _userRepository.SaveChangesAsync();
                return result;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
}
