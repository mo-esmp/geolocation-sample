using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GeographySample.Core.Distance
{
    public class LocationDistanceCreateCommand : IRequest
    {
        [Range(-180, 180)]
        public double Longitude1 { get; set; }

        [Range(-90, 90)]
        public double Latitude1 { get; set; }

        [Range(-180, 180)]
        public double Longitude2 { get; set; }

        [Range(-90, 90)]
        public double Latitude2 { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
    }
}