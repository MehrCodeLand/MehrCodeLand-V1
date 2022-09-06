using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Username Required")]
        [Display(Name = "Username")]
        public string Usernmae { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "RePassword Required")]
        [Display(Name = "RePassword")]
        [Compare("Password" , ErrorMessage = "The Password didn't match.")]
        public string RePassword { get; set; }

    }
}
