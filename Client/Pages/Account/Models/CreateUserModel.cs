namespace Gejms.Client.Pages.Account.Models;

using Gejms.Client.Shared.Utilities;
using System.ComponentModel.DataAnnotations;
public class CreateUserModel
{
    [Required, CustomUsernameValidation(3, 8)]
    public string? Username { get; set; }

    [Required]
    public string? Password { get; set; }

    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string? RepeatedPassword { get; set; }
}
