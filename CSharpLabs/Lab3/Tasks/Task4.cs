using CSharpLabs.Lab3.Common;

namespace CSharpLabs.Lab3.Tasks;

internal static class Task4
{
    public static void RunExample()
    {
        Run(20, 3, 2, new Random(3));
    }

    public static void Run(int count, int m, int n, Random? random = null)
    {
        string source = Lab3Files.GetDataPath("numbers.bin");
        string destination = Lab3Files.GetDataPath("filtered.bin");
        FileTasks.FillBinaryNumbers(source, count, random ?? Random.Shared);
        FileTasks.FilterBinaryNumbers(source, destination, m, n);
        Console.WriteLine($"Исходный файл: {source}\n{FileTasks.ReadBinaryNumbers(source)}");
        Console.WriteLine($"m = {m}, n = {n}");
        Console.WriteLine($"Результат: {destination}\n{FileTasks.ReadBinaryNumbers(destination)}");
    }
}
