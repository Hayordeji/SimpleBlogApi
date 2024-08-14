using API.Dto.PostDto;
using API.Interface;
using API.Mapper;
using API.Models;
using Microsoft.EntityFrameworkCore;
using SimpleBlogApi.Data;
using SimpleBlogApi.Models;
using System.Linq;

namespace API.Repository
{
    public class UserPostRepository : IUserPostRepository
    {
        private readonly ApplicationDbContext _context;
        public UserPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserPost> CreateUserPost(UserPost userPost)
        {
            await _context.UserPosts.AddAsync(userPost);
            await _context.SaveChangesAsync();
            return userPost;
        }

        public async Task<List<Post>> GetUserPosts(AppUser user)
        {
            return await _context.UserPosts.Where(u => u.UserId == user.Id).Select(userPost => new Post
            {
                Id = userPost.PostId,
                Title = userPost.Post.Title,
                Content = userPost.Post.Content,
                CreatedOn = userPost.Post.CreatedOn,
            }).ToListAsync();
        }

        
    }
}
