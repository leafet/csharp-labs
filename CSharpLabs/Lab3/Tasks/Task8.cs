using CSharpLabs.Lab3.Common;

namespace CSharpLabs.Lab3.Tasks;

internal static class Task8
{
    public static void RunExample()
    {
        string[] lines = { "Багаж готов", "билет куплен", "", "Борис в пути", " Буква после пробела", "Анна дома" };
        Run(lines, 'Б');
    }

    public static void Run(string[] lines, char firstCharacter)
    {
        string source = Lab3Files.GetDataPath("lines.txt");
        string destination = Lab3Files.GetDataPath("selected_lines.txt");
        FileTasks.FillTextLines(source, lines);
        FileTasks.FilterLines(source, destination, firstCharacter);
        Console.WriteLine($"Исходный файл: {source}");
        Console.Write(FileTasks.ReadText(source));
        Console.WriteLine($"Строки на символ «{firstCharacter}», с учётом регистра: {destination}");
        string result = FileTasks.ReadText(destination);
        if (result.Length == 0)
        {
            Console.WriteLine("(пусто)");
        }
        else
        {
            Console.WriteLine(result);
        }
    }
}
