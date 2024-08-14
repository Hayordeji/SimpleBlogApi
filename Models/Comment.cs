using API.Models;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlogApi.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int PostId { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public List<UserComment> UserComments { get; set; } = new List<UserComment>();
    }
}
