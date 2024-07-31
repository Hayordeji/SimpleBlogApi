using API.Dto.CommentDto;
using API.Helpers;
using SimpleBlogApi.Models;

namespace API.Interface
{
    public interface ICommentRepository
    {
        public Task<List<Comment>> GetAllComments(QueryObject query);
        public Task<List<Comment>> GetPostComments(int postId);
        public Task<Comment> GetComment(int id);
        public Task<Comment> CreateComment(Comment comment);
        public Task<Comment> UpdateComment(Comment comment, int id);
        public Task<bool> CommentExists(int id);
        public Task<Comment> DeleteComment(int id);



    }
}
