using API.Dto.PostDto;
using API.Helpers;
using API.Interface;
using API.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/Post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepo;
        public PostController(IPostRepository postRepo)
        {
            _postRepo = postRepo;
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
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newPost = postModel.ToPostCreateDto();
            if (newPost == null)
            {
                return BadRequest("Couldn't add post");
            }
            await _postRepo.CreatePost(newPost);

            return Ok("Post Created Successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostDto postModel,[FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var postToUpdate = await _postRepo.UpdatePost(postModel,id);
            if (postToUpdate == null)
            {
                return BadRequest("Couldn't update post");
            }

            return Ok("Post Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var postToDelete = await _postRepo.DeletePost(id);
            if (postToDelete == null)
            {
                return NotFound();
            }

            return Ok("Post Deleted Successfully");
        }
    }    
}
