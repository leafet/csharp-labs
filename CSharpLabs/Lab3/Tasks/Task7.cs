using CSharpLabs.Lab3.Common;

namespace CSharpLabs.Lab3.Tasks;

internal static class Task7
{
    public static void RunExample()
    {
        Run(12, 4, new Random(3));
    }

    public static void Run(int count, int perLine, Random? random = null)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 2);
        ArgumentOutOfRangeException.ThrowIfLessThan(perLine, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(perLine, count);

        string path = Lab3Files.GetDataPath("task7.txt");
        FileTasks.FillTextNumbers(path, count, perLine, random ?? Random.Shared);
        Console.WriteLine($"Исходный файл: {path}");
        Console.Write(FileTasks.ReadText(path));

        Console.WriteLine($"Минимальный элемент = {FileTasks.FindMinimum(path)}");
    }
}
