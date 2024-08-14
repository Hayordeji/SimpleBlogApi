using SimpleBlogApi.Models;

namespace API.Models
{
    public class UserPost
    {
        public string UserId { get; set; }
        public int PostId { get; set; }
        public AppUser User { get; set; }
        public Post Post { get; set; }
    }
}
