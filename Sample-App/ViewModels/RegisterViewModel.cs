using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sample_App.ViewModels
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is reqd")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is reqd")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm pw do not match")]
        public string ConfirmPassword { get; set; }

        public string DrivingLicense { get; set; }
    }
}
