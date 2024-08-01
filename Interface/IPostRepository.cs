using API.Dto.PostDto;
using API.Helpers;
using SimpleBlogApi.Models;

namespace API.Interface
{
    public interface IPostRepository
    {
        //Get all  Posts
        public Task<List<Post>> GetPosts(QueryObject query);
        //Get single post
        public Task<Post> GetPostById(int id);
        //Create Post
        public Task<Post> CreatePost(Post post);
        //Update Post
        public Task<Post> UpdatePost(UpdatePostDto post, int id);
        //Delete Post
        public Task<Post> DeletePost(int id);

        public Task<bool> PostExists(int id);
       


    }
}
