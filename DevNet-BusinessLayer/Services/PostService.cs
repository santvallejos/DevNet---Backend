using DevNet_BusinessLayer.DTOs;
using DevNet_BusinessLayer.Interfaces;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Interfaces;
using DevNet_DataAccessLayer.Models;
using DevNet_DataAccessLayer.Repositories;
using DevNet_WebAPI.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_BusinessLayer.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly DevnetDBContext _context;

        public PostService(IPostRepository postRepository, DevnetDBContext context)
        {
            _postRepository = postRepository;
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetUserPostsAsync(Guid userId)
        {
            return await _postRepository.GetUserPosts(userId);
        }

        public async Task<Post> GetPostAsync(Guid postId)
        {
            return await _postRepository.GetPostById(postId);
        }

        public async Task<bool> NewPostAsync(NewPostDto postDto)
        {
            Post newPost = new Post
            {
                Id = Guid.NewGuid(),
                Text = postDto.Text,
                MediaUrl = postDto.MediaUrl,
                UserId = postDto.UserId,
                CreatedAt = DateTime.UtcNow,
                CommentCount = 0,
                LikeCount = 0,
                UpdatedAt = null        
            };

            var result = await _postRepository.AddAsync(newPost);

            if (result) await _postRepository.SaveChangesAsync();
            return result;
        }

        public async Task<bool> DeletePostAsync(Guid postId, DeletePostDto postDto)
        {
            if (postId != postDto.PostId) return false;

            var post = await _postRepository.GetPostById(postId);

            if (post.UserId != postDto.UserId) return false;

            var result = await _postRepository.DeleteAsync(post);

            if (result) await _postRepository.SaveChangesAsync();
            return result;
        }

        public async Task<bool> EditPostAsync(Guid postId, EditPostDto postDto)
        {
            if (postId != postDto.Id) return false;

            var post = await _postRepository.GetPostById(postId);

            if (post.UserId != postDto.UserId) return false;

            post.Text = postDto.Text;
            post.MediaUrl = postDto.MediaUrl;
            post.UpdatedAt = DateTime.UtcNow;

            var result = await _postRepository.UpdateAsync(post);

            if (result) await _postRepository.SaveChangesAsync();
            return result;
        }
    }
}
