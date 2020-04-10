using System;

namespace GeographySample.DistanceCalculator
{
    public class DistanceCalculator
    {
        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            var radLat1 = Math.PI * lat1 / 180;
            var radLat2 = Math.PI * lat2 / 180;

            var theta = lon1 - lon2;
            var radTheta = Math.PI * theta / 180;

            var dist =
                Math.Sin(radLat1) * Math.Sin(radLat2) + Math.Cos(radLat1) *
                Math.Cos(radLat2) * Math.Cos(radTheta);

            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;

                case 'N': //Nautical Miles
                    return dist * 0.8684;

                case 'M': //Miles
                    return dist;
            }

            return dist;
        }
    }
}