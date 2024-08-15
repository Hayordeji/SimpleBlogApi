using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Dto.CommentDto
{
    public class UpdateCommentDto
    {
        //i removed id,postid and userid
        public string Content { get; set; }
        
    }
}
