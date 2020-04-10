using MediatR;
using System.ComponentModel.DataAnnotations;

namespace GeographySample.Core.User
{
    public class UserLoginCommand : IRequest<UserEntity>
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}