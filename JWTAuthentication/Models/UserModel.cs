﻿using System;
using System.Collections.Generic;

namespace JWTAuthentication.Authentication
{
    public class UserModel
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string ProfilePhoto { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get { return FirstName + " " + LastName; } }
        public DateTime DateOfBirth { get; set; }
    }
}