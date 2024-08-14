using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Dto.CommentDto
{
    public class UpdateCommentDto
    {
      
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        
    }
}
