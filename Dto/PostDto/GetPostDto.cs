using API.Dto.CommentDto;
using SimpleBlogApi.Models;

namespace API.Dto.PostDto
{
    public class GetPostDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<GetCommentDto>? Comments { get; set; }
    }
}
