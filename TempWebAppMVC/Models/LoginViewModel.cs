using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TempWebAppMVC.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(150)]
        [Display(Name = "Login: ")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(200, MinimumLength = 5)]
        [Display(Name = "Hasło: ")]
        public string Password { get; set; }
    }
}