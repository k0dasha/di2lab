using System.CommandLine;
using System.Drawing;
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
    ListsOfFigures figures = new ListsOfFigures();

    if (parseResult.GetValue(fileOption) is string filePath)
    {
        string[] objects = ReadFile(filePath);
        figures = GetObjects(objects);
    }
    if (parseResult.GetValue(operOption) is string operationToDo)
    {
        switch (operationToDo)
        {
            case "print":
                ToPrint(figures);
                break;
            case "count":
                ToCount(figures);
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
static ListsOfFigures GetObjects(string[] objects)
{
    ListsOfFigures listsOfFigures = new ListsOfFigures();

    foreach (string obj in objects)
    {
        Match figureName = Regex.Match(obj, @"^(\w+)\(");
        switch (figureName.Groups[1].Value)
        {
            case "Point":
                var p = CreatePoint(obj);
                if (p != null)
                    listsOfFigures.Points.Add(p);
                break;

            case "Line":
                Match linePoints = Regex.Match(obj, @"Line\((Point\([^)]*\)),\s*(Point\([^)]*\))\)$");
                var point1 = CreatePoint(linePoints.Groups[1].Value);
                var point2 = CreatePoint(linePoints.Groups[2].Value);
                if (point1 != null && point2 != null)
                {
                    Line l = new Line(point1, point2);
                    listsOfFigures.Lines.Add(l);
                }

                break;

            case "Circle":
                Match circleOps = Regex.Match(obj, @"Circle\((Point\([^)]*\)),\s*(\d+(?:\.\d+)?)\)$");
                var point = CreatePoint(circleOps.Groups[1].Value);

                if (int.TryParse(circleOps.Groups[2].Value, out int r) && point != null)
                {
                    Circle c = new Circle(point, r);
                    listsOfFigures.Circles.Add(c);
                }
                break;

            default:
                break;
        }
    }
    return listsOfFigures;
}
static Point CreatePoint(string pointString)
{
    Match xy = Regex.Match(pointString, @"Point\((\d+(?:\.\d+)?),\s*(\d+(?:\.\d+)?)\)$");

    if (xy.Success)
    {
        double x = Convert.ToDouble(xy.Groups[1].Value, CultureInfo.InvariantCulture);
        double y = Convert.ToDouble(xy.Groups[2].Value, CultureInfo.InvariantCulture);
        Point point = new Point(x, y);
        return point;
    }
    else
    {
        return null;
    }
}

static void ToCount(ListsOfFigures lists)
{
    Console.WriteLine($"Точки: {lists.Points.Count}");
    Console.WriteLine($"Линии: {lists.Lines.Count}");
    Console.WriteLine($"Круги: {lists.Circles.Count}");
}
static void ToPrint(ListsOfFigures lists)
{
    string s = "";

    foreach (Point p in lists.Points)
        s += $"Point({p.X}, {p.Y}); ";
    foreach (Line l in lists.Lines)
        s += $"Line(Point({l.begin.X}, {l.begin.Y}), Point({l.end.X}, {l.end.Y})); ";
    foreach (Circle c in lists.Circles)
        s += $"Circle(Point({c.Center.X}, {c.Center.Y}), {c.Radius}); ";
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
public class ListsOfFigures
{
    public List<Point> Points { get; set; } = new List<Point>();
    public List<Line> Lines { get; set; } = new List<Line>();
    public List<Circle> Circles { get; set; } = new List<Circle>();
}