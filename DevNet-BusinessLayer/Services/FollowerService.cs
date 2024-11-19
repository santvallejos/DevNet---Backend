using DevNet_BusinessLayer.DTOs;
using DevNet_BusinessLayer.Interfaces;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Interfaces;
using DevNet_DataAccessLayer.Models;
using DevNet_WebAPI.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_BusinessLayer.Services
{
    public class FollowerService : IFollowerService
    {
        private readonly IFollowerRepository _followerRepository;
        private readonly DevnetDBContext _context;

        public FollowerService(IFollowerRepository followerRepository, DevnetDBContext context)
        {
            _followerRepository = followerRepository;
            _context = context;
        }

        public async Task<IEnumerable<Follower>> GetUserFollowersAsync(Guid userId)
        {
            return await _followerRepository.GetUserFollowers(userId);
        }

        public async Task<IEnumerable<Follower>> GetUserFollowsAsync(Guid userId)
        {
            return await _followerRepository.GetUserFollows(userId);
        }

        public async Task<bool> AddFollowAsync(AddFollowerDto followerDto)
        {
            Follower follower = new Follower
            {
                FollowedId = followerDto.FollowerId,
                FollowerId = followerDto.FollowedId,
                FollowedAt = DateTime.Now
            };

            var result = await _followerRepository.AddAsync(follower);

            return result;
        }

        public async Task<bool> UnfollowAsync(UnfollowDto unfollowDto)
        {
            var userFollows = await _followerRepository.GetUserFollows(unfollowDto.FollowerId);              
            var follow = userFollows.FirstOrDefault(f => f.FollowedId == unfollowDto.FollowedId);

            if (follow == null)
            {
                return false;
            }

            var result = await _followerRepository.DeleteAsync(follow);
            return result;
        }

    }
}
