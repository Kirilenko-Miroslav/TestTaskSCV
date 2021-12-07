using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskSCV
{
    public class Circle : Point
    {
        public double rad { get; set; }

        public bool IsInside(Point point) => GetGeoDistance(point) < rad;
        public bool IsCrossing(Point p1, Point p2) => !IsInside(p1) && IsInside(p2);
    }
}
