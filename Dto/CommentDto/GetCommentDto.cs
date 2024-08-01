using SimpleBlogApi.Models;

namespace API.Dto.CommentDto
{
    public class GetCommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } 
        public DateTime CreatedOn { get; set; } 
        
    }
}
