using API.Dto.CommentDto;
using API.Models;
using SimpleBlogApi.Models;

namespace API.Interface
{
    public interface IUserCommentRepository
    {
        Task<List<Comment>> GetUserComments(AppUser user);
        Task<UserComment> CreateUserComment(UserComment userComment);
    }
}
