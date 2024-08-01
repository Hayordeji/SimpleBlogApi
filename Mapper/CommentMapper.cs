using API.Dto.CommentDto;
using SimpleBlogApi.Models;

namespace API.Mapper
{
    public static class CommentMapper
    {
        public static GetCommentDto ToGetCommentDto(this Comment commentModel)
        {
            return new GetCommentDto
            {
                Id = commentModel.Id,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
            };
        }

        public static Comment ToCreateCommentDto(this CreateCommentDto commentModel, int postId)
        {
            return new Comment
            {
                Id = commentModel.Id,
                Content = commentModel.Content,
                PostId = postId
            };
        }

        public static Comment ToUpdateCommentDto(this UpdateCommentDto commentModel)
        {
            return new Comment
            {
                Id = commentModel.Id,
                Content = commentModel.Content,
            };
        }

    }
}
