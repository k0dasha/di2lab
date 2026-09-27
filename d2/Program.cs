using System.CommandLine;
using System.Text.RegularExpressions;

string[] objects;
List<object>[] objectsArr = new List<object>[3]; //POINTS, LINES, CIRCLES

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
    if (parseResult.GetValue(fileOption) is string filePath)
    {
        objects = ReadFile(filePath);
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
    string[] objects = File.ReadAllLines(path);
    return objects;
}
static List<object>[] GetObjects(string[] objects)
{
    List<object>[] objectsArr = new List<object>[3];
    foreach (string obj in objects)
    {
        if (Regex.IsMatch(obj, "^Line\\("))
        {
            Match match = Regex.Match(obj, "Line\\((.*)\\)");
            string points = match.Groups[1].Value;
            var matches = Regex.Matches(points, @"Point\([^)]*\)");
            var point1 = CreatePoint(matches[0].Value);
            var point2 = CreatePoint(matches[1].Value);
            Line line = new Line(point1, point2);
            objectsArr[1].Add(line);

        }
        if (Regex.IsMatch(obj, "^Circle\\("))
        {
            Match match = Regex.Match(obj, "Circle\\((.*)\\)");
            string point_and_radius = match.Groups[1].Value;
        }
    }
    return null;
    
}
static Point CreatePoint(string pointString)
{
    Match match = Regex.Match(pointString, "Point\\((.*)\\)");
    string xy = match.Groups[1].Value;
    Match match1 = Regex.Match(xy, "(.*), (.*)");
    int x = Convert.ToInt32(match1.Groups[1].Value);
    int y = Convert.ToInt32(match1.Groups[2].Value);
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
                    s += $"Point({p.X}, {p.Y}) ";
                    break;

                case Line l:
                    s+= $"Line(Point({l.begin.X}, {l.begin.Y}), Point({l.end.X}, {l.end.Y})) ";
                    break;

                case Circle c:
                    s += $"Circle(Point({c.Center.X}, {c.Center.Y}), {c.Radius}) ";
                    break;
            }
        }
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