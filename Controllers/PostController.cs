using API.Dto.PostDto;
using API.Extensions;
using API.Helpers;
using API.Interface;
using API.Mapper;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/Post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserPostRepository _userPostRepo;
        public PostController(IPostRepository postRepo, UserManager<AppUser> userManager, IUserPostRepository userPostRepo)
        {
            _postRepo = postRepo;
            _userManager = userManager;
            _userPostRepo = userPostRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] QueryObject query)
        {
           var posts = await _postRepo.GetPosts(query);
           var postDto = posts.Select(s => s.ToPostGetDto());
           return Ok(postDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var post = await _postRepo.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post.ToPostGetDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //check if user is logged in
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
            {
                return Forbid("You are not logged in");
            }

            //create new post
            var newPost = postModel.ToPostCreateDto();
            if (newPost == null)
            {
                return BadRequest("Couldn't add post");
            }
            newPost.UserId = appUser.Id;
            await _postRepo.CreatePost(newPost);

            //create userPost object
            var userPost = new UserPost 
            { 
                UserId = appUser.Id,
                PostId = newPost.Id,
            };
            await _userPostRepo.CreateUserPost(userPost);
            
            return Ok("Post Created Successfully");
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostDto postModel,[FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //check if user is logged in 
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
            {
                return Forbid("You are not logged in");
            }

            //check if user is allowed to update post
            var post = await _postRepo.GetPostById(id);
            if (post.UserId != appUser.Id)
            {
                return Forbid("The post was not created by this user.");
            }

            //update post
            var updatedPost =await _postRepo.UpdatePost(postModel,id);
            if (updatedPost == null)
            {
                return BadRequest("Couldn't update post");
            }

            return Ok("Post Updated");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //check if user is logged in
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
            {
                return Forbid("You are not logged in");
            }

            //check if usr is allowed to delete post
            var post = await _postRepo.GetPostById(id);
            if (post.UserId != appUser.Id)
            {
                return Forbid("The post was not created by this user.");
            }

            //delete post
            var postToDelete = await _postRepo.DeletePost(id);
            if (postToDelete == null)
            {
                return NotFound();
            }

            return Ok("Post Deleted Successfully");
        }
    }    
}
