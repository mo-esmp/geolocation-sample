using GeographySample.Core;
using GeographySample.Core.Distance;
using GeographySample.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using System.Threading;
using System.Threading.Tasks;

namespace GeographySample.Infrastructure.GeoDistance
{
    public class GeoLocationCommandHandler : IRequestHandler<LocationDistanceCreateCommand>
    {
        private readonly GeographyDbContext _context;
        private readonly IMessageSession _messagingSession;

        public GeoLocationCommandHandler(GeographyDbContext context, IMessageSession messagingSession)
        {
            _context = context;
            _messagingSession = messagingSession;
        }

        public async Task<Unit> Handle(LocationDistanceCreateCommand request, CancellationToken cancellationToken)
        {
            var userExist = await _context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);
            if (!userExist)
                throw new DomainException("User not found");

            var sendOptions = new SendOptions();
            sendOptions.SetDestination("GeographySample.Receiver");
            var response = await _messagingSession.Request<CalculateDistanceResponse>(
                new CalculateDistanceRequest
                {
                    Latitude1 = request.Latitude1,
                    Longitude1 = request.Longitude1,
                    Latitude2 = request.Latitude2,
                    Longitude2 = request.Longitude2
                },
                sendOptions);

            var geoDistance = new GeoDistanceEntity
            {
                Latitude1 = request.Latitude1,
                Longitude1 = request.Longitude1,
                Latitude2 = request.Latitude2,
                Longitude2 = request.Longitude2,
                Distance = response.Distance,
                UserId = request.UserId,
            };

            _context.GeoDistances.Add(geoDistance);

            return Unit.Value;
        }
    }
}