using API.Dto.CommentDto;
using API.Helpers;
using API.Interface;
using Microsoft.EntityFrameworkCore;
using SimpleBlogApi.Data;
using SimpleBlogApi.Models;
using System.Linq;

namespace API.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPostRepository _postRepo;
        public CommentRepository(ApplicationDbContext context, IPostRepository postRepo)
        {
            _context = context;
            _postRepo = postRepo;
        }
        public async Task<Comment> GetComment(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            return comment;
        }

        public async Task<List<Comment>> GetAllComments(QueryObject query)
        {
            var comments = _context.Comments.AsQueryable();
            int pageSize = 4;
            int defaultNumber = Math.Abs(query.PageNumber);
            var skipNumber = (query.PageNumber - 1) * pageSize;
            if (query.PageNumber > 0)
            {
                return await comments.Skip(skipNumber).Take(pageSize).ToListAsync();
            }
            else
            {
                return await comments.Skip(0).Take(pageSize).ToListAsync();
            }
        }

        public async Task<List<Comment>> GetPostComments(int postId)
        {
            if (!await _postRepo.PostExists(postId))
            {
                return null;
            }

            var comments = await _context.Comments.Where(c => c.PostId == postId).ToListAsync();

            return comments;
        }

        public async Task<Comment> CreateComment(Comment comment)
        {
            //create comment
            await _context.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment> UpdateComment(Comment comment, int id)
        {
            var commentToUpdate = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

            commentToUpdate.Content = comment.Content;
            _context.Update(commentToUpdate);
            await _context.SaveChangesAsync();
            return commentToUpdate;
        }

        public async Task<bool> CommentExists(int id)
        {
            return await _context.Comments.AnyAsync(c => c.Id == id);
        }

        public async Task<Comment> DeleteComment(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}
