using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ForgotPasswordVm
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Enter Your Email")]
        [EmailAddress(ErrorMessage = "Email Is Not Valid!")]
        public string Email { get; set; }
    }
}
