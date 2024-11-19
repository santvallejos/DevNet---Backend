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
    public class CommentService : ICommentService
    {

        private readonly ICommentRepository _commentRepository;
        private readonly DevnetDBContext _context;

        public CommentService(ICommentRepository commentRepository, DevnetDBContext context)
        {
            _commentRepository = commentRepository;
            _context = context;
        }

        public bool CommentExist(Guid id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            return await _commentRepository.GetAllAsync();
        }

        public async Task<Comment> GetCommentAsync(Guid id)
        {
            return await _commentRepository.GetByIdAsync(id);
        }

        public async Task<bool> EditCommentAsync(Guid id, EditCommentDto commentDto)
        {
            if (id != commentDto.Id) return false;

            var comment = await _commentRepository.GetByIdAsync(id);
            
            comment.Text = commentDto.Text;
            
            return await _commentRepository.UpdateAsync(comment);
        }

        public async Task<bool> PostCommentAsync(PostCommentDto commentDto)
        {
            Comment comment = new Comment
            {
                Id = Guid.NewGuid(),
                Text = commentDto.Text,
                CreatedAt = DateTime.Now,
                PostId = commentDto.PostId,
                UserId = commentDto.UserId
            };

            return await _commentRepository.AddAsync(comment);
        }

        public async Task<bool> DeleteCommentAsync(Guid id, DeleteCommentDto commentDto)
        {
            Comment comment = await _commentRepository.GetByIdAsync(commentDto.Id);


            if (commentDto.UserId != comment.UserId) return false;

            var result = await _commentRepository.DeleteAsync(comment);
            return result;
        }
    }
}
