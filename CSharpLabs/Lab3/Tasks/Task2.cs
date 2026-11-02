using CSharpLabs.Lab3.Common;

namespace CSharpLabs.Lab3.Tasks;

internal static class Task2
{
    public static void RunExample()
    {
        Run(new Matrix(new long[,]
        {
            { 1, 2, 3, 4, 5 },
            { 6, 7, 8, 9, 10 },
            { 11, 12, 13, 14, 15 },
            { 16, 17, 18, 19, 20 }
        }));
    }

    public static void Run(Matrix matrix)
    {
        Console.WriteLine("Исходная матрица:");
        Console.WriteLine(matrix);
        Matrix block = matrix.FindMaximumBlock(out int row, out int column, out long sum);
        Console.WriteLine($"Начало блока: строка {row + 1}, столбец {column + 1}; сумма = {sum}.");
        Console.WriteLine(block);
    }
}
