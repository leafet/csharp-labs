using System.Globalization;
using CSharpLabs.Common;

namespace CSharpLabs.Lab4.Points;

internal static class Program
{
    public static void Main()
    {
        try
        {
            while (true)
            {
                ConsoleInput.ClearScreen();
                Console.WriteLine("Лабораторная работа №4 — вариант 3, задания 6–7");
                Console.WriteLine("1 — Ввести точки");
                Console.WriteLine("2 — Запустить пример");
                Console.WriteLine("0 — Выход");
                int choice = ConsoleInput.ReadInt("Выберите пункт: ", 0, 2);

                if (choice == 0)
                {
                    return;
                }

                Point a = choice == 2 ? new Point(-2.75, 2) : ReadPoint("A");
                Point b = choice == 2 ? new Point(0.25, 6) : ReadPoint("B");
                int offset = choice == 2 ? 3 : ConsoleInput.ReadInt("Целое смещение: ", int.MinValue, int.MaxValue);
                PrintOperations(a, b, offset);
                ConsoleInput.WaitForContinue();
            }
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("\nВвод завершён.");
        }
    }

    private static Point ReadPoint(string name)
    {
        double x = ReadCoordinate($"{name}.X: ");
        double y = ReadCoordinate($"{name}.Y: ");
        return new Point(x, y);
    }

    private static double ReadCoordinate(string prompt)
    {
        string value = string.Empty;
        double coordinate = 0;

        while (true)
        {
            value = ConsoleInput.ReadRequiredText(prompt).Replace(',', '.');

            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out coordinate) &&
                double.IsFinite(coordinate))
            {
                return coordinate;
            }

            Console.WriteLine("Введите конечное вещественное число.");
        }
    }

    private static void PrintOperations(Point a, Point b, int offset)
    {
        Point origin = new Point();
        Console.WriteLine("\nЗадание 6 — класс Point");
        Console.WriteLine($"Конструктор без параметров: {origin}");
        origin.X = 1;
        origin.Y = -1;
        Console.WriteLine($"После установки X и Y: {origin}");
        Console.WriteLine($"Точки из конструктора с параметрами: A = {a}, B = {b}");
        Console.WriteLine($"A.DistanceTo(B) = {a.DistanceTo(b)}");
        Console.WriteLine("\nЗадание 7 — перегрузка операций");
        Point shifted = a;
        Console.WriteLine($"++A = {++shifted}");
        shifted = a;
        Console.WriteLine($"--A = {--shifted}");

        try
        {
            Console.WriteLine($"(int)A = {(int)a}");
        }
        catch (OverflowException)
        {
            Console.WriteLine("(int)A: целая часть X вне диапазона Int32.");
        }

        double y = a;
        Console.WriteLine($"Неявное приведение A к double = {y}");
        Console.WriteLine($"A + B = {a + b}");
        Console.WriteLine($"A + ({offset}) = {a + offset}");
        Console.WriteLine($"({offset}) + A = {offset + a}");
        Console.WriteLine($"Исходная A = {a}");
    }
}
