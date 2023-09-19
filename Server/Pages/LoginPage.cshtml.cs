using Gejms.Server.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Gejms.Server.Pages;

public class LoginPageModel : PageModel
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;

    public LoginPageModel(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }
    [BindProperty]
    public bool Loading { get; set; } = false;

    [BindProperty]
    public UserDTO22 UserInput { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }
    public class UserDTO22
    {
        [Required, StringLength(10, ErrorMessage = "Username must be 5 to 10 letters", MinimumLength = 5)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

}

