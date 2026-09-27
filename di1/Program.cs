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
            Circle cc = new Circle(new Point(3, 4), 7);
            Console.WriteLine(cc.Radius);
        }
    }
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x; Y = y;
        }
    }
    public class Circle
    {
        public Point Center { get; set; }
        public int Radius { get; set; }

        public Circle(Point centerPoint, int radius)
        {
            Center = centerPoint;
            Radius = radius;
        }
    }
    public class Line
    {
        public Point begin { get; set; }
        public Point end { get; set; }
        public Line(Point x1y1, Point x2y2)
        {
            begin = x1y1;
            end = x2y2;
        }
    }
}
