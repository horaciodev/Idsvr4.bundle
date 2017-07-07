using System.ComponentModel.DataAnnotations;

namespace userhub.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
