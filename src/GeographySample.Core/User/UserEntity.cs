using Microsoft.AspNetCore.Identity;

namespace GeographySample.Core.User
{
    public class UserEntity : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}