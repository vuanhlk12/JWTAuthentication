﻿using System;
using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Authentication
{
    public class ChangeInfoModel
    {
        public string Account { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public bool ChangePassword { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Token { get; set; }
    }
}
