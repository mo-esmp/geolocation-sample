using GeographySample.Core;
using GeographySample.Core.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace GeographySample.Infrastructure.User
{
    public class UserCommandHandler :
        IRequestHandler<UserCreateCommand, UserEntity>,
        IRequestHandler<UserLoginCommand, UserEntity>
    {
        private readonly UserManager<UserEntity> _userManager;

        public UserCommandHandler(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserEntity> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var user = new UserEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
                return user;

            throw new DomainException(string.Join("\r\n", result.Errors));
        }

        public async Task<UserEntity> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user == null)
                return null;

            var isValid = await _userManager.CheckPasswordAsync(user, request.Password);

            return isValid ? user : null;
        }
    }
}