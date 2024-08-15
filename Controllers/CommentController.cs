using API.Dto.CommentDto;
using API.Extensions;
using API.Helpers;
using API.Interface;
using API.Mapper;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API.Controllers
{
    [Route("api/Comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IPostRepository _postRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserCommentRepository _userCommentRepo;
        public CommentController(ICommentRepository commentRepo, IPostRepository postRepo, UserManager<AppUser> userManager,
            IUserCommentRepository userCommentRepo)
        {
            _commentRepo = commentRepo;
            _postRepo = postRepo;
            _userManager = userManager;
            _userCommentRepo = userCommentRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments([FromQuery] QueryObject query)
        {
            var comments = await _commentRepo.GetAllComments(query);
            var commentsDto = comments.Select(c => c.ToGetCommentDto());
            return Ok(commentsDto);
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetCommentById([FromRoute]int commentId) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _commentRepo.GetComment(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToGetCommentDto());
        }

        [HttpGet("{postId}/comments")]
        public async Task<IActionResult> GetPostComments([FromRoute] int postId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var comments = await _commentRepo.GetPostComments(postId);
            if (comments == null)
            {
                return NotFound("Comments could not be found");
            }
            var commentsDto = comments.Select(c => c.ToGetCommentDto());
            

            return Ok(commentsDto);
        }

        [HttpPost("{postId}")]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto commentDto, [FromRoute] int postId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //validation to check if the user is authorized
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null)
            {
                return Forbid("You are not logged in");
            }

            //Check if post exists
            if (!await _postRepo.PostExists(postId))
            {
                return NotFound("Post could not be found");
            }
            
            //create new comment
            var newComment = commentDto.ToCreateCommentDto(postId);
            if (newComment == null)
            {
                return BadRequest("Could not add comment");
            }
            newComment.UserId = appUser.Id;
            await _commentRepo.CreateComment(newComment);

            //create comment object
            var userComment = new UserComment { 
                UserId = newComment.UserId,
                CommentId = newComment.Id,
            };
            await _userCommentRepo.CreateUserComment(userComment);

            return Ok(newComment.ToGetCommentDto());
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult>UpdateComment([FromBody]UpdateCommentDto commentDto, [FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //validation to check if the user is authorized
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null)
            {
                return Forbid();
            }

            if (!await _commentRepo.CommentExists(id))
            {
                return NotFound("Comment Not Found");
            }

            var comment = await _commentRepo.GetComment(id);
            if (comment.UserId != appUser.Id)
            {
                return Forbid("Comment was not created by this user");
            }
            var commentModel = await _commentRepo.UpdateComment(commentDto.ToUpdateCommentDto(), id);
            return Ok("Updated Successfully");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveComment([FromRoute] int id) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //validation to check if the user is authorized
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null)
            {
                return Forbid("You are not logged in");
            }

            if (!await _commentRepo.CommentExists(id))
            {
                return NotFound();
            }

            var commentToDelete = await _commentRepo.GetComment(id);
            if (commentToDelete.UserId != appUser.Id)
            {
                return Forbid("Comment was not created by this user");
            }
            await _commentRepo.DeleteComment(id);
            return Ok("Comment successfully deleted");


        }
    }
}

