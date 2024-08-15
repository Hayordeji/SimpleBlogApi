using API.Dto.CommentDto;
using API.Interface;
using API.Models;
using Microsoft.EntityFrameworkCore;
using SimpleBlogApi.Data;
using SimpleBlogApi.Models;

namespace API.Repository
{
    public class UserCommentRepository : IUserCommentRepository
    {
        private readonly ApplicationDbContext _context;
        public UserCommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserComment> CreateUserComment(UserComment userComment)
        {
            var commment = await _context.UserComments.AddAsync(userComment);
            await _context.SaveChangesAsync();
            return userComment;
        }

        public async Task<UserComment> DeleteUserComment(int id)
        {
            var userComment = await _context.UserComments.FirstOrDefaultAsync(c => c.CommentId == id);
            if (userComment == null)
            {
                return null;
            }
            _context.Remove(userComment);
            await _context.SaveChangesAsync();
            return userComment;
        }

        public async Task<List<Comment>> GetUserComments(AppUser user)
        {
            return await _context.UserComments.Where(u => u.UserId == user.Id).Select(userComment => new Comment
            {
                Id = userComment.CommentId,
                Content = userComment.Comment.Content,
                CreatedOn = userComment.Comment.CreatedOn,
                PostId = userComment.Comment.PostId,
            }).ToListAsync();
        }
    }
}
