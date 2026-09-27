using System.CommandLine;

string[] descriptions = new string[3] { "Точки", "Линии", "Круги" };

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
    List<object>[] objectsArr = new List<object>[3]; //POINTS, LINES, CIRCLES

    if (parseResult.GetValue(fileOption) is string filePath)
    {
        s = ReadFile(filePath);
        objectsArr = GetObjects(s);
    }
    if (parseResult.GetValue(operOption) is string operationToDo)
    {
        switch (operationToDo)
        {
            case "print":
                ToPrint(objectsArr, descriptions);
                break;
            case "count":
                ToCount(objectsArr, descriptions);
                break;
            default:
                Console.WriteLine($"Операции '{operationToDo}' не существует");
                break;
            
        }
    }
    Console.WriteLine(s);
});

ParseResult result = rootCommand.Parse(args);
return result.Invoke();

static string ReadFile(string path)
{
string objects = File.ReadAllText(path);
return objects;
}
static List<object>[] GetObjects(string objectsString)
{
    return null;
}
static void ToCount(List<object>[] lists, string[] figuresNames)
{
    for (int i = 0; i < lists.Length; i++)
    {
        var count = lists[i].Count();
        Console.WriteLine($"{figuresNames[i]}: {count}");
    }
}
static void ToPrint(List<object>[] lists, string[] figuresNames)
{
    string s = "";

    for (int i = 0; i < lists.Length; i++)
    {
        foreach (var item in lists[i])
        {
            switch (i)
            {
                case 0:
                    var p = (Point)item;
                    s += $"Point({p.X}, {p.Y}) ";
                    break;

                case 1:
                    var l = (Line)item;
                    s+= $"Line(Point({l.begin.X}, {l.begin.Y}), Point({l.end.X}, {l.end.Y})) ";
                    break;

                case 2:
                    var c = (Circle)item;
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