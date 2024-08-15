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

        

        
    }
}
