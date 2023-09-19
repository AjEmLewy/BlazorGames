using Gejms.Server.Entities;
using Gejms.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gejms.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var username = HttpContext.User?.Identity?.Name;
            
            return Ok(username);
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserDTO userDTO)
        {
            var user = await userManager.FindByNameAsync(userDTO.Username);
            if (user == null)
                return BadRequest();
            await signInManager.PasswordSignInAsync(user, userDTO.Password, true, false);
            return Ok();
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            if(userDTO.Username.Length>8 || userDTO.Username.Length<3)
                return BadRequest("Wrong Username length");
            
            var user = await userManager.FindByNameAsync(userDTO.Username);
            if (user != null)
                return BadRequest("Such user already exists");
            user = new User() { UserName = userDTO.Username };
            var result = await userManager.CreateAsync(user, userDTO.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, true);

                return Created("/api/account/Register", null);
            }
            else
            {
                return BadRequest(string.Join(" | ", result.Errors.Select(x => x.Description).ToArray()));
            }
            
        }
    }
}
