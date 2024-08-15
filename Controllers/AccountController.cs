using API.Dto.Account;
using API.Interface;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if ( await _tokenService.EmailExists(registerModel.Email))
                {
                    return BadRequest("Email already Exists");
                }
                if (await _tokenService.UserNameExists(registerModel.Username)) 
                {
                    return BadRequest("Username already Exists");
                }
                var newUser = new AppUser
                {
                    UserName = registerModel.Username,
                    Email = registerModel.Email,
                };

                var createdUser = await _userManager.CreateAsync(newUser, registerModel.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(newUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(new NewUserDto
                        {
                            Username = newUser.UserName,
                            Email = newUser.Email,
                            JWTtoken = await _tokenService.CreateToken(newUser)

                        });
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginModel.UsernameOrEmail.ToLower()
            || x.Email == loginModel.UsernameOrEmail.ToLower());
           
            if (user == null)
            {
                return Unauthorized("Wrong Username/ Password");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Wrong Username/Password");
            }
            return Ok(new NewUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                JWTtoken = await _tokenService.CreateToken(user)
            });


        }
    }
}
