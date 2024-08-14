using API.Dto.CommentDto;
using API.Extensions;
using API.Interface;
using API.Mapper;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/UserComment")]
    [ApiController]
    public class UserCommentController : ControllerBase
    {
        private readonly IUserCommentRepository _UserCommentRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICommentRepository _commentRepo;
        public UserCommentController(IUserCommentRepository userCommentRepo, UserManager<AppUser> userManager, ICommentRepository commentRepo)
        {
            _UserCommentRepo = userCommentRepo;
            _commentRepo = commentRepo;
            _userManager = userManager; 
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserComments()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);           
            var userComments = await _UserCommentRepo.GetUserComments(appUser);
            
            return Ok(userComments);
            
        }

        [HttpPost("{postId}")]
        [Authorize]
        public async Task<IActionResult> CreateUserComment([FromBody] CreateCommentDto commentDto, [FromRoute] int postId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //Get user claim
            var user = User.GetUsername();
            //find the user that matches the user claim
            var appUser = await _userManager.FindByNameAsync(user);
            //create comment
            var commentMap = commentDto.ToCreateCommentDto(postId);
            var newComment = await _commentRepo.CreateComment(commentMap);
            //check if comment is null
            if (newComment == null)
            {
                return BadRequest("Comment is null");
            }
            //create usercomment object
            var newUserComment = new UserComment
            {
                UserId = appUser.Id,
                CommentId = newComment.Id,
            };

            return Created();
        }
    }
}
