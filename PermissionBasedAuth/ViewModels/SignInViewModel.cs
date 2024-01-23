using System.ComponentModel.DataAnnotations;

namespace PermissionBasedAuth.ViewModels
{
    public class SignInViewModel
    {
        [Required]
        [Display(Name = "username/email")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
