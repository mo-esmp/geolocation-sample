using MediatR;
using System.ComponentModel.DataAnnotations;

namespace GeographySample.Core.User
{
    public class UserCreateCommand : IRequest<UserEntity>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}