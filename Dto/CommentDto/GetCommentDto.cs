using SimpleBlogApi.Models;

namespace API.Dto.CommentDto
{
    public class GetCommentDto
    {
        public string Content { get; set; } 
        public DateTime CreatedOn { get; set; } 
        
    }
}
