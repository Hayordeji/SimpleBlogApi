using API.Models;
using SimpleBlogApi.Models;

namespace API.Dto.CommentDto
{
    public class GetCommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UserId { get; set; } 
        public int PostId { get; set; }
        public DateTime CreatedOn { get; set; } 
        
        
    }
}
