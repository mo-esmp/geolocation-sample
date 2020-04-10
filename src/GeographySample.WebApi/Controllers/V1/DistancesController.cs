using GeographySample.Core;
using GeographySample.Core.Distance;
using GeographySample.Core.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeographySample.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DistancesController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public DistancesController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserRegisterDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IEnumerable<GeoLocationDto>> Get()

        {
            var dtos = await _mediator.Send(new GeoLocationGetsQuery { UserId = User.Identity.GetUserId() });

            return dtos;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserRegisterDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post(LocationDistanceCreateCommand command)

        {
            if (!ModelState.IsValid)
                return ValidationProblem();

            command.UserId = User.Identity.GetUserId();
            await _mediator.Send(command);
            await _unitOfWork.CommitAsync();

            return Ok();
        }
    }
}