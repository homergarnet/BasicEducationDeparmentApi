using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BasicEducationDepartment.Models.DTO
{
    public class UsersSigninDTO
    {

        [Required(ErrorMessage = "username is required.")]
        public string AccountUser { get; set; }

        [Required(ErrorMessage = "password is required.")]
        public string AccountPassword { get; set; }
        public string AccountType { get; set; }
        public string DateTimeCreated { get; set; }
        public string DateTimeUpdated { get; set; }

    }

    public class UsersSignupDTO
    {

        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string Year { get; set; }
        public string Password { get; set; }

    }

    public class UserChangePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string AccountType { get; set; }
        public int AccountId { get; set; }
    }

}