using Localization;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = Resources.FormErrors_FieldIsRequired)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = Resources.FormErrors_FieldIsRequired)]
        public string Password { get; set; }
    }
}
