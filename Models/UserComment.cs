using SimpleBlogApi.Models;

namespace API.Models
{
    public class UserComment
    {
        public string UserId { get; set; }
        public int CommentId { get; set; }
        public AppUser User { get; set; }
        public Comment Comment {  get; set; }
    }
}
