using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;

namespace di1
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Option<string> fileOption = new Option<string>("-file")
            {
                Description = "Указать файл, с которого будут читаться объекты"
            };
            Option<string> operOption = new Option<string>("-oper")
            {
                Description = "Задать операцию над объектом"
            };

            RootCommand rootCommand = new RootCommand("CLI App for lab2");
            rootCommand.Options.Add(fileOption);
            rootCommand.Options.Add(operOption);

            rootCommand.SetAction(parseResult =>
            {
                string s = "";
                if(parseResult.GetValue(fileOption) is string filePath)
                {
                    s = ReadFile(filePath);
                }
                Console.WriteLine(s);
            });

            ParseResult result = rootCommand.Parse(args);
            return result.Invoke();
        }
    
    public static string ReadFile(string path)
    {
        string objects = ReadFile(path);
            return objects;
    }
    public static void GetObjects(string objectsString)
    {
        
    }
    public static void ToCount()
    {

    }
    public static void ToPrint()
    {

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
