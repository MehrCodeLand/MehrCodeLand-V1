using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ResetPasswordVm
    {
        public string ActiveCode { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter password")]
        public string Password { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter Password")]
        [Compare("Password")]
        public string RePassword { get; set; }
    }
}
