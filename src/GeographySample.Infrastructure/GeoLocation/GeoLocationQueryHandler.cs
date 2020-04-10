using GeographySample.Core.Distance;
using GeographySample.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeographySample.Infrastructure.GeoLocation
{
    public class GeoLocationQueryHandler : IRequestHandler<GeoLocationGetsQuery, IEnumerable<GeoLocationDto>>
    {
        private readonly GeographyDbContext _context;

        public GeoLocationQueryHandler(GeographyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GeoLocationDto>> Handle(GeoLocationGetsQuery request, CancellationToken cancellationToken)
        {
            return await _context.GeoDistances
                .Where(gd => gd.UserId == request.UserId)
                .Select(gd => new GeoLocationDto
                {
                    Id = gd.Id,
                    Longitude1 = gd.Longitude1,
                    Latitude1 = gd.Latitude1,
                    Longitude2 = gd.Longitude2,
                    Latitude2 = gd.Latitude2,
                    Distance = gd.Distance
                })
                .ToListAsync(cancellationToken);
        }
    }
}