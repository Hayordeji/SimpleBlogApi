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

       
    }
}
