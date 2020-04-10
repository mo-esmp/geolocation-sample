using NServiceBus;

namespace GeographySample.Core.Distance
{
    public class CalculateDistanceRequest : IMessage
    {
        public double Longitude1 { get; set; }

        public double Latitude1 { get; set; }

        public double Longitude2 { get; set; }

        public double Latitude2 { get; set; }
    }
}