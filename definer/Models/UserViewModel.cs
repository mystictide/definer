using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace definer.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "you'll need a username.")]
        [StringLength(35, ErrorMessage = "35 characters max.", MinimumLength = 3)]
        [Remote("CheckExistingUsername", "Account", HttpMethod = "POST", ErrorMessage = "that sounds familiar, uh... try again?")]
        public string Username { get; set; }
        [Required(ErrorMessage = "please, enter an e-mail address.")]
        [RegularExpression(@"^([\w.-]+)@([\w-]+)((.(\w){2,3})+)$", ErrorMessage = "your argument is invalid.")]
        [Remote("CheckExistingEmail", "Account", HttpMethod = "POST", ErrorMessage = "heard of this before. nice try.")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "trust me, you need this.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "your argument is invalid.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "confirm your password.")]
        [Compare("Password", ErrorMessage = "passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public bool RememberMe { get; set; }
    }
}
