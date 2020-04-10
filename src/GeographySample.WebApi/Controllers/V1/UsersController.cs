using GeographySample.Core.User;
using GeographySample.WebApi.Authentication.Jwt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GeographySample.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public UsersController(IMediator mediator, JwtTokenGenerator jwtTokenGenerator)
        {
            _mediator = mediator;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost, AllowAnonymous]
        [ProducesResponseType(typeof(UserRegisterDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(UserCreateCommand command)

        {
            if (!ModelState.IsValid)
                return ValidationProblem();

            var user = await _mediator.Send(command);
            var token = _jwtTokenGenerator.GenerateJwtToken(user.Id);

            return Ok(new UserRegisterDto { JwtToken = token, UserId = user.Id });
        }

        [HttpPost("login"), AllowAnonymous]
        [ProducesResponseType(typeof(UserRegisterDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(UserLoginCommand command)

        {
            if (!ModelState.IsValid)
                return ValidationProblem();

            var user = await _mediator.Send(command);
            if (user == null)
            {
                ModelState.AddModelError("BadUserNamePassword", "Username or password is not valid");
                return ValidationProblem();
            }

            var token = _jwtTokenGenerator.GenerateJwtToken(user.Id);

            return Ok(new UserRegisterDto { JwtToken = token, UserId = user.Id });
        }
    }
}