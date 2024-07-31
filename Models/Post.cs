using System.ComponentModel.DataAnnotations;

namespace SimpleBlogApi.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int Like { get; set; } = 0;
        public int Dislike { get; set; } = 0;
        public List<Comment>? Comments { get; set; }

    }
}
