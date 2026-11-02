using CSharpLabs.Common;
using CSharpLabs.Lab3.Common;

namespace CSharpLabs.Lab3.Tasks;

internal static class Task1
{
    public static void Run()
    {
        Console.WriteLine("Первая матрица: ввод по столбцам сверху вниз.");
        int n = ConsoleInput.ReadInt("Число строк n (1–20): ", 1, 20);
        int m = ConsoleInput.ReadInt("Число столбцов m (1–20): ", 1, 20);
        Matrix first = new Matrix(n, m);
        Run(first);
    }

    public static void RunExample()
    {
        Console.WriteLine("Готовая первая матрица: числа 1–15 расположены по столбцам.");
        Run(new Matrix(new long[,]
        {
            { 1, 6, 11 },
            { 2, 7, 12 },
            { 3, 8, 13 },
            { 4, 9, 14 },
            { 5, 10, 15 }
        }), new Random(3));
    }

    public static void Run(Matrix first, Random? random = null)
    {
        int n = first.Rows;
        Console.WriteLine($"Размеры: первая матрица {n}×{first.Columns}, вторая и третья {n}×{n}.");
        Console.WriteLine("Первая матрица:");
        Console.WriteLine(first);
        Console.WriteLine("Вторая матрица: выше побочной [-12;4565], на ней и ниже [-1024;1024]:");
        Console.WriteLine(new Matrix(n, random ?? Random.Shared));
        Console.WriteLine("Третья матрица: заполнение по рисунку, без случайных чисел:");
        Console.WriteLine(new Matrix(n));
    }
}
