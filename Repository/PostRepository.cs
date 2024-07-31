using API.Dto.PostDto;
using API.Helpers;
using API.Interface;
using API.Mapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SimpleBlogApi.Data;
using SimpleBlogApi.Models;
using System.Linq;

namespace API.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
       
        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Post> CreatePost(Post post)
        {
            var postToCreate = await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post; 
        }

        public async Task<Post> DeletePost(int id)
        {
            var postToDelete = await _context.Posts.FirstOrDefaultAsync(c => c.Id == id);
            if (postToDelete == null)
            {
                return null;
            }

            _context.Remove(postToDelete);
            await _context.SaveChangesAsync();
            return postToDelete;
        }

        public async Task<Post> GetPostById(int id)
        {
            var post = await _context.Posts.Include(c => c.Comments).FirstOrDefaultAsync(p => p.Id == id);
            return post;
        }

        public async Task<List<Post>> GetPosts(QueryObject query)
        {
            var posts =  _context.Posts.Include(c => c.Comments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
               posts = _context.Posts.Where(s => s.Title.Contains(query.Keyword));
            }

            int skipNumber = (query.PageNumber - 1) * query.PageSize;
            if (query.PageNumber > 0 && query.PageSize > 0)
            {
                return await posts.Skip(skipNumber).Take(query.PageSize).ToListAsync();
            }
            else
            {
                return await posts.ToListAsync();
            }

            
        }

        public async Task<Post> UpdatePost(UpdatePostDto post, int id)
        {
            var postToUpdate = await _context.Posts.FirstOrDefaultAsync(c => c.Id == id);
            if (postToUpdate == null)
            {
                return null;
            }

            postToUpdate.Title = post.Title;
            postToUpdate.Content = post.Content;

            await _context.SaveChangesAsync();
            return postToUpdate;
        }

        public async Task<bool> PostExists(int id)
        {
            return await _context.Posts.AnyAsync(c => c.Id == id);
        }

        public async Task<Post> LikePost(int id)
        {
            var postToLike = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (postToLike == null)
            {
                return null;
            }
            postToLike.Like++;
            await _context.SaveChangesAsync();
            return postToLike;
        }

        public async Task<Post> DislikePost(int id)
        {
            var postToLike = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (postToLike == null)
            {
                return null;
            }
            postToLike.Dislike++;
            await _context.SaveChangesAsync();
            return postToLike;
        }
    }   
}
