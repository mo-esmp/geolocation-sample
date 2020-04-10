using GeographySample.Core.Distance;
using NServiceBus;
using System.Threading.Tasks;

namespace GeographySample.DistanceCalculator
{
    public class CalculateDistanceHandler : IHandleMessages<CalculateDistanceRequest>
    {
        public Task Handle(CalculateDistanceRequest message, IMessageHandlerContext context)
        {
            var response = new CalculateDistanceResponse
            {
                Distance = DistanceCalculator.DistanceTo(message.Latitude1, message.Longitude1, message.Latitude2,
                    message.Longitude2)
            };

            return context.Reply(response);
        }
    }
}