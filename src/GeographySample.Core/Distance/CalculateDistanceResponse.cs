using NServiceBus;

namespace GeographySample.Core.Distance
{
    public class CalculateDistanceResponse : IMessage
    {
        public double Distance { get; set; }
    }
}