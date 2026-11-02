using CSharpLabs.Lab3.Common;

namespace CSharpLabs.Lab3.Tasks;

internal static class Task6
{
    public static void RunExample()
    {
        Run(12, new Random(3));
    }

    public static void Run(int count, Random? random = null)
    {
        string path = Lab3Files.GetDataPath("task6.txt");
        FileTasks.FillTextNumbers(path, count, 1, random ?? Random.Shared);
        Console.WriteLine($"Исходный файл: {path}");
        Console.Write(FileTasks.ReadText(path));

        Console.WriteLine($"Максимум − минимум = {FileTasks.FindRange(path)}");
    }
}
