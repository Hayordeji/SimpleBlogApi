using API.Dto.PostDto;
using API.Models;
using SimpleBlogApi.Models;

namespace API.Interface
{
    public interface IUserPostRepository
    {
        Task<List<Post>> GetUserPosts(AppUser user);
        Task <UserPost> CreateUserPost(UserPost userPost);
        Task<UserPost> DeleteUserPost(int id);


    }
}
