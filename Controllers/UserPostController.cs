using API.Dto.PostDto;
using API.Extensions;
using API.Interface;
using API.Mapper;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace API.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserPostController : ControllerBase
    {
        private readonly UserManager<AppUser> _user;
        private readonly IPostRepository _postRepo;
        private readonly IUserPostRepository _userPostRepo;
        public UserPostController(UserManager<AppUser> user, IPostRepository postRepo,IUserPostRepository userPostRepo)
        {
            _user = user;
            _postRepo = postRepo;
            _userPostRepo = userPostRepo;
           
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPosts()
        {
            var username = User.GetUsername();
            var appUser = await _user.FindByNameAsync(username);
            var userPosts = await _userPostRepo.GetUserPosts(appUser);

            return Ok(userPosts);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUserPost([FromBody] CreatePostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //gets the user
            var user = User.GetUsername();
            var appUser = await _user.FindByNameAsync(user);

            //Map the PostDto to an object
            var newPost = postDto.ToPostCreateDto();
            if (newPost == null)
            {
                return BadRequest("Post is empty");
            }

            //creates the post
            var createdPost = await _postRepo.CreatePost(newPost);
            if (createdPost == null)
            {
                return BadRequest("Could not add post");
            }

            //Initialize the UserPost Object
            var userPost = new UserPost
            {
                UserId = appUser.Id,
                PostId = createdPost.Id,
            };

            //Add the UserPost object to the database
            await _userPostRepo.CreateUserPost(userPost);
            if (userPost == null)
            {
                return StatusCode(500, "Could not add UserPost");
            }
            return Created();

        }

        
    }
}
