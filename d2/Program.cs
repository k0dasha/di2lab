using System.CommandLine;
using System.Globalization;
using System.Text.RegularExpressions;

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
    List<object>[] objectsArr = new List<object>[3]
    {
        new List<object>(),
        new List<object>(),
        new List<object>()
    };

    if (parseResult.GetValue(fileOption) is string filePath)
    {
        string[] objects = ReadFile(filePath);
        objectsArr = GetObjects(objects);
    }
    if (parseResult.GetValue(operOption) is string operationToDo)
    {
        switch (operationToDo)
        {
            case "print":
                ToPrint(objectsArr);
                break;
            case "count":
                ToCount(objectsArr);
                break;
            default:
                Console.WriteLine($"Операции '{operationToDo}' не существует");
                break;
        }
    }
});

ParseResult result = rootCommand.Parse(args);
return result.Invoke();

static string[] ReadFile(string path)
{
    return File.ReadAllLines(path);
}
static List<object>[] GetObjects(string[] objects)
{
    List<object>[] objectsArr = new List<object>[3]
    {
        new List<object>(),
        new List<object>(),
        new List<object>()
    };

    foreach (string obj in objects)
    {
        Match figureName = Regex.Match(obj, @"^(\w+)\(");
        switch (figureName.Groups[1].Value)
        {
            case "Point":
                var p = CreatePoint(obj);
                objectsArr[0].Add(p);
                break;

            case "Line":
                Match linePoints = Regex.Match(obj, @"Line\((Point\([^)]*\)),\s*(Point\([^)]*\))\)$");
                var point1 = CreatePoint(linePoints.Groups[1].Value);
                var point2 = CreatePoint(linePoints.Groups[2].Value);
                Line l = new Line(point1, point2);
                objectsArr[1].Add(l);

                break;

            case "Circle":
                Match circleOps = Regex.Match(obj, @"Circle\((Point\([^)]*\)),\s*(\d+(?:\.\d+)?)\)$");
                var point = CreatePoint(circleOps.Groups[1].Value);
                var r = Convert.ToInt32(circleOps.Groups[2].Value);
                Circle c = new Circle(point, r);
                objectsArr[2].Add(c);

                break;

            default:
                break;
        }
    }
    return objectsArr;
    
}
static Point CreatePoint(string pointString)
{
    Match xy = Regex.Match(pointString, @"Point\((\d+(?:\.\d+)?),\s*(\d+(?:\.\d+)?)\)$");

    double x = Convert.ToDouble(xy.Groups[1].Value, CultureInfo.InvariantCulture);
    double y = Convert.ToDouble(xy.Groups[2].Value, CultureInfo.InvariantCulture);
    Point point = new Point(x, y);
    return point;
}

static void ToCount(List<object>[] lists)
{
    string[] figuresNames = new string[3] { "Точки", "Линии", "Круги" };

    for (int i = 0; i < lists.Length; i++)
    {
        var count = lists[i].Count();
        Console.WriteLine($"{figuresNames[i]}: {count}");
    }
}
static void ToPrint(List<object>[] lists)
{
    string s = "";

    for (int i = 0; i < lists.Length; i++)
    {
        foreach (var item in lists[i])
        {
            switch (item)
            {
                case Point p:
                    s += $"Point({p.X}, {p.Y}); ";
                    break;

                case Line l:
                    s+= $"Line(Point({l.begin.X}, {l.begin.Y}), Point({l.end.X}, {l.end.Y})); ";
                    break;

                case Circle c:
                    s += $"Circle(Point({c.Center.X}, {c.Center.Y}), {c.Radius}); ";
                    break;
            }
        }
    }
    Console.WriteLine(s);
}
    public class Point
{
    public double X { get; set; }
    public double Y { get; set; }
    public Point(double x, double y)
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