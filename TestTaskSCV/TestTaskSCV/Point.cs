using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskSCV
{
    public class Point
    {
        public double lon { get; set; }
        public double lat { get; set; }
        public double GetGeoDistance(Point point)
        {
            const double gpsEarthRadius = 6371000;
            const double pi = Math.PI;
            var latRad1 = lat * pi / 180;
            var lonRad1 = lon * pi / 180;
            var latRad2 = point.lat * pi / 180;
            var lonRad2 = point.lon * pi / 180;
            var angle = Math.Cos(latRad1) * Math.Cos(latRad2) * Math.Pow(Math.Sin(((lonRad1 - lonRad2)
             / 2)), 2) + Math.Pow(Math.Sin((double)((latRad1 - latRad2) / 2)), 2);
            angle = 2 * Math.Asin(Math.Sqrt(angle));
            return Math.Abs(angle * gpsEarthRadius);
        }
    }
}
