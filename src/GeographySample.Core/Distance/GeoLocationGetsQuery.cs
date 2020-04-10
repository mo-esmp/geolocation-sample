using MediatR;
using System.Collections.Generic;

namespace GeographySample.Core.Distance
{
    public class GeoLocationGetsQuery : IRequest<IEnumerable<GeoLocationDto>>
    {
        public string UserId { get; set; }
    }
}