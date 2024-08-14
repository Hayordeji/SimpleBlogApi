using API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBlogApi.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public List<Comment>? Comments { get; set; }
        public List<UserPost> UserPost { get; set; } = new List<UserPost>();
        
        public string UserId { get; set; }
        public AppUser User { get; set; }

    }
}
