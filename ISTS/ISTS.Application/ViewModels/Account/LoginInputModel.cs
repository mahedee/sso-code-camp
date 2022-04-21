using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.ViewModels.Account
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "User Name can not be empty")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password can not be empty")]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}
