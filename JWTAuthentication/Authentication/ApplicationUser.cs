using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;

namespace JWTAuthentication.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string Gender { get; set; }
        public string ProfilePhoto { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
