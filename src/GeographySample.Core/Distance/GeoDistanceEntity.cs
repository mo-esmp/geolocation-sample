using GeographySample.Core.User;

namespace GeographySample.Core.Distance
{
    public class GeoDistanceEntity
    {
        public int Id { get; set; }

        public double Longitude1 { get; set; }

        public double Latitude1 { get; set; }

        public double Longitude2 { get; set; }

        public double Latitude2 { get; set; }

        public double Distance { get; set; }

        public string UserId { get; set; }

        public UserEntity User { get; set; }
    }
}