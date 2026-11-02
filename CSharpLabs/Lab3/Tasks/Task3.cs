using CSharpLabs.Lab3.Common;

namespace CSharpLabs.Lab3.Tasks;

internal static class Task3
{
    public static void RunExample()
    {
        Run(
            new Matrix(new long[,] { { 1, 2 }, { 3, 4 } }),
            new Matrix(new long[,] { { 5, 6 }, { 7, 8 } }),
            new Matrix(new long[,] { { 9, 10 }, { 11, 12 } }));
    }

    public static void Run(Matrix a, Matrix b, Matrix c)
    {
        Console.WriteLine($"A:\n{a}\nB:\n{b}\nC:\n{c}");
        Matrix result = a.Transpose() + 2 * b + c.Transpose();
        Console.WriteLine($"Aᵀ + 2*B + Cᵀ:\n{result}");
    }
}
