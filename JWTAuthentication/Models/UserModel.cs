using System;
using System.Collections.Generic;

namespace JWTAuthentication.Authentication
{
    public class UserModel
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        private string phoneNumber { get; set; }
        public string PhoneNumber
        {
            get
            {
                return phoneNumber ?? "";
            }
            set
            {
                phoneNumber = value;
            }
        }
        public string Gender { get; set; }
        public string ProfilePhoto { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get { return FirstName + " " + LastName; } }
        public DateTime DateOfBirth { get; set; }

        public List<StoreModel> FollowStores { get; set; }
    }
}
