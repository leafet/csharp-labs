using System.Globalization;
using CSharpLabs.Common;
using CSharpLabs.Lab3.Tasks;

namespace CSharpLabs.Lab3.Common;

internal static class Lab3Input
{
    public static void Run(int task)
    {
        switch (task)
        {
            case 1:
                Task1.Run();
                break;
            case 2:
                Task2.Run(ReadMatrix("Исходная матрица", 3));
                break;
            case 3:
                ReadExpression();
                break;
            case 4:
                int count = ConsoleInput.ReadInt("Количество чисел (0–100): ", 0, 100);
                int m = ReadNonzero("Делитель m: ");
                int n = ReadNonzero("Делитель n: ");
                Task4.Run(count, m, n);
                break;
            case 5:
                Passenger[] passengers = ReadPassengers();
                decimal mass = ReadMass("Порог m, кг: ", false);
                Task5.Run(passengers, mass);
                break;
            case 6:
            case 7:
                int minimumCount;

                if (task == 6)
                {
                    minimumCount = 1;
                }
                else
                {
                    minimumCount = 2;
                }
                int numberCount = ConsoleInput.ReadInt($"Количество чисел ({minimumCount}–100): ", minimumCount, 100);
                int maximumPerLine = Math.Min(10, numberCount);
                int perLine;

                if (task == 6)
                {
                    perLine = 1;
                }
                else
                {
                    perLine = ConsoleInput.ReadInt($"Чисел в строке (2–{maximumPerLine}): ", 2, maximumPerLine);
                }

                if (task == 6)
                {
                    Task6.Run(numberCount);
                }
                else
                {
                    Task7.Run(numberCount, perLine);
                }
                break;
            case 8:
                char firstCharacter = ReadCharacter();
                string[] lines = ReadLines();
                Task8.Run(lines, firstCharacter);
                break;
        }
    }

    private static void ReadExpression()
    {
        int rows = ConsoleInput.ReadInt("Число строк A и C (1–20): ", 1, 20);
        int columns = ConsoleInput.ReadInt("Число столбцов A и C (1–20): ", 1, 20);
        Console.WriteLine("Матрица A, ввод по столбцам:");
        Matrix a = new Matrix(rows, columns);
        Console.WriteLine("Матрица B, размеры обратные размерам A:");
        Matrix b = new Matrix(columns, rows);
        Console.WriteLine("Матрица C:");
        Matrix c = new Matrix(rows, columns);
        Task3.Run(a, b, c);
    }

    private static Passenger[] ReadPassengers()
    {
        int count = ConsoleInput.ReadInt("Количество пассажиров (0–20): ", 0, 20);
        Passenger[] passengers = new Passenger[count];
        string name;
        int itemsCount;
        BaggageItem[] items;
        string itemName;

        for (int i = 0; i < count; i++)
        {
            name = ConsoleInput.ReadRequiredText($"Имя пассажира {i + 1}: ");
            itemsCount = ConsoleInput.ReadInt("Количество единиц багажа (0–20): ", 0, 20);
            items = new BaggageItem[itemsCount];

            for (int j = 0; j < itemsCount; j++)
            {
                itemName = ConsoleInput.ReadRequiredText($"Название единицы {j + 1}: ");
                items[j] = new BaggageItem(itemName, ReadMass("Масса, кг: "));
            }

            passengers[i] = new Passenger(name, items);
        }

        return passengers;
    }

    private static char ReadCharacter()
    {
        string symbol;

        do
        {
            Console.Write("Начальный символ (ровно один, пробел допустим): ");
            symbol = Console.ReadLine() ?? throw new EndOfStreamException();

            if (symbol.Length != 1)
            {
                Console.WriteLine("Введите ровно один символ.");
            }
        }
        while (symbol.Length != 1);

        return symbol[0];
    }

    private static string[] ReadLines()
    {
        int count = ConsoleInput.ReadInt("Количество строк (0–30): ", 0, 30);
        string[] lines = new string[count];

        for (int i = 0; i < count; i++)
        {
            Console.Write($"Строка {i + 1}: ");
            lines[i] = Console.ReadLine() ?? throw new EndOfStreamException();
        }

        return lines;
    }

    private static Matrix ReadMatrix(string title, int minimum = 1)
    {
        Console.WriteLine(title);
        int rows = ConsoleInput.ReadInt($"Число строк ({minimum}–20): ", minimum, 20);
        int columns = ConsoleInput.ReadInt($"Число столбцов ({minimum}–20): ", minimum, 20);
        return new Matrix(rows, columns);
    }

    private static int ReadNonzero(string prompt)
    {
        int value;

        while (true)
        {
            value = ConsoleInput.ReadInt(prompt, int.MinValue, int.MaxValue);

            if (value != 0)
            {
                return value;
            }

            Console.WriteLine("Делитель не должен быть равен нулю.");
        }
    }

    private static decimal ReadMass(string prompt, bool requirePositive = true)
    {
        string text;
        decimal value;

        while (true)
        {
            text = ConsoleInput.ReadRequiredText(prompt).Replace(',', '.');

            if (decimal.TryParse(text, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out value) && (!requirePositive || value > 0))
            {
                return value;
            }

            if (requirePositive)
            {
                Console.WriteLine("Введите положительную массу; дробную часть отделите точкой или запятой.");
            }
            else
            {
                Console.WriteLine("Введите число; дробную часть отделите точкой или запятой.");
            }
        }
    }
}
