using Microsoft.AspNetCore.Identity;
using SimpleBlogApi.Models;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class AppUser :IdentityUser
    {
        public List<UserPost> UserPosts { get; set; } = new List<UserPost>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Post> Posts { get; set; } = new List<Post>();

    }
}
