using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace di1
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }
    public class Point
    {
        int x;
        int y;
    }
    public class Circle
    {
        Point center;
        Line radius;
        Point no_center;
        public Circle()
        {
            center = new Point();
            radius = new Line(center, no_center);
        }
    }
    public class Line
    {
        Point begin;
        Point end;
        public Line(Point x1y1, Point x2y2)
        {
            begin = x1y1;
            end = x2y2;
        }
    }
}
