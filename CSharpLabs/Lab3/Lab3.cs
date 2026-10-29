using System.Globalization;
using System.Text;

namespace CSharpLabs.Lab3;

/// <summary>Меню и примеры восьми заданий лабораторной работы №3, вариант 3.</summary>
public class Lab3
{
    /// <summary>Запускает примеры из меню и режим ручного ввода.</summary>
    public void Run()
    {
        while (true)
        {
            ConsoleInput.ClearScreen();
            Console.WriteLine("Лабораторная работа №3 — вариант 3");
            Console.WriteLine("1 — Заполнение трёх матриц");
            Console.WriteLine("2 — Подмассив 3×3 с максимальной суммой");
            Console.WriteLine("3 — Матричное выражение Aᵀ + 2*B + Cᵀ");
            Console.WriteLine("4 — Фильтрация бинарного файла");
            Console.WriteLine("5 — Багаж пассажиров и XML-сериализация");
            Console.WriteLine("6 — Разность максимума и минимума в файле");
            Console.WriteLine("7 — Минимум в файле с несколькими числами в строке");
            Console.WriteLine("8 — Строки, начинающиеся с заданного символа");
            Console.WriteLine("9 — Ручной ввод параметров заданий");
            Console.WriteLine("0 — Назад");
            int choice = ConsoleInput.ReadInt("Выберите задание: ", 0, 9);

            if (choice == 0)
            {
                return;
            }

            bool manual = choice == 9;

            if (manual)
            {
                choice = ConsoleInput.ReadInt("Номер задания (1–8): ", 1, 8);
            }

            try
            {
                RunTask(choice, manual);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine($"Ошибка данных: {exception.Message}");
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine($"Ошибка обработки: {exception.Message}");
            }
            catch (IOException exception) when (exception is not EndOfStreamException)
            {
                Console.WriteLine($"Ошибка файла: {exception.Message}");
            }
            catch (UnauthorizedAccessException exception)
            {
                Console.WriteLine($"Нет доступа к файлу: {exception.Message}");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Результат выходит за диапазон целого числа long.");
            }

            ConsoleInput.WaitForContinue();
        }
    }

    private static void RunTask(int choice, bool manual)
    {
        Console.WriteLine($"Задание {choice}, вариант 3");

        switch (choice)
        {
            case 1:
                RunFilling(manual);
                break;
            case 2:
                RunMaximumBlock(manual);
                break;
            case 3:
                RunExpression(manual);
                break;
            case 4:
                RunBinary(manual);
                break;
            case 5:
                RunPassengers(manual);
                break;
            case 6:
            case 7:
                RunTextNumbers(choice, manual);
                break;
            case 8:
                RunTextLines(manual);
                break;
        }
    }

    private static void RunFilling(bool manual)
    {
        Matrix first;
        int size = 5;

        if (manual)
        {
            first = ReadMatrix("Первая матрица");
            size = ConsoleInput.ReadInt("Размер квадратных матриц (1–20): ", 1, 20);
        }
        else
        {
            Console.WriteLine("Ввод по столбцам: 1, 2, 3, 4, 5, 6.");
            first = new Matrix(new long[,] { { 1, 3, 5 }, { 2, 4, 6 } });
        }

        Console.WriteLine("Первая матрица:");
        Console.WriteLine(first);
        Console.WriteLine("Вторая матрица: выше побочной [-12;4565], на ней и ниже [-1024;1024]:");
        Console.WriteLine(new Matrix(size, new Random(3)));
        Console.WriteLine("Третья матрица:");
        Console.WriteLine(new Matrix(size));
    }

    private static void RunMaximumBlock(bool manual)
    {
        Matrix matrix = manual ? ReadMatrix("Исходная матрица", 3) : new Matrix(new long[,]
        {
            { 1, 2, 3, 4, 5 },
            { 6, 7, 8, 9, 10 },
            { 11, 12, 13, 14, 15 },
            { 16, 17, 18, 19, 20 }
        });
        Console.WriteLine("Исходная матрица:");
        Console.WriteLine(matrix);
        Matrix block = matrix.FindMaximumBlock(out int row, out int column, out long sum);
        Console.WriteLine($"Начало блока: строка {row + 1}, столбец {column + 1}; сумма = {sum}.");
        Console.WriteLine(block);
    }

    private static void RunExpression(bool manual)
    {
        Matrix a;
        Matrix b;
        Matrix c;

        if (manual)
        {
            int rows = ConsoleInput.ReadInt("Число строк A и C (1–20): ", 1, 20);
            int columns = ConsoleInput.ReadInt("Число столбцов A и C (1–20): ", 1, 20);
            Console.WriteLine("Матрица A, ввод по столбцам:");
            a = new Matrix(rows, columns);
            Console.WriteLine("Матрица B, размеры обратные размерам A:");
            b = new Matrix(columns, rows);
            Console.WriteLine("Матрица C:");
            c = new Matrix(rows, columns);
        }
        else
        {
            a = new Matrix(new long[,] { { 1, 2 }, { 3, 4 } });
            b = new Matrix(new long[,] { { 5, 6 }, { 7, 8 } });
            c = new Matrix(new long[,] { { 9, 10 }, { 11, 12 } });
        }

        Console.WriteLine($"A:\n{a}\nB:\n{b}\nC:\n{c}");
        Matrix result = a.Transpose() + 2 * b + c.Transpose();
        Console.WriteLine($"Aᵀ + 2*B + Cᵀ:\n{result}");
    }

    private static void RunBinary(bool manual)
    {
        int count = manual ? ConsoleInput.ReadInt("Количество чисел (0–100): ", 0, 100) : 20;
        int m = manual ? ReadNonzero("Делитель m: ") : 3;
        int n = manual ? ReadNonzero("Делитель n: ") : 2;
        string source = DataPath("numbers.bin");
        string destination = DataPath("filtered.bin");
        FileTasks.FillBinaryNumbers(source, count, new Random(3));
        FileTasks.FilterBinaryNumbers(source, destination, m, n);
        Console.WriteLine($"Исходный файл: {source}\n{FileTasks.ReadBinaryNumbers(source)}");
        Console.WriteLine($"m = {m}, n = {n}");
        Console.WriteLine($"Результат: {destination}\n{FileTasks.ReadBinaryNumbers(destination)}");
    }

    private static void RunPassengers(bool manual)
    {
        Passenger[] passengers =
        {
            new Passenger("Анна", new[] { new BaggageItem("Сумка", 4) }),
            new Passenger("Борис", new[] { new BaggageItem("Чемодан", 12), new BaggageItem("Коробка", 2) }),
            new Passenger("Вера", new[] { new BaggageItem("Рюкзак", 5) })
        };
        decimal m = 5;

        if (manual)
        {
            int count = ConsoleInput.ReadInt("Количество пассажиров (0–20): ", 0, 20);
            passengers = new Passenger[count];

            for (int i = 0; i < count; i++)
            {
                string name = ConsoleInput.ReadRequiredText($"Имя пассажира {i + 1}: ");
                int itemsCount = ConsoleInput.ReadInt("Количество единиц багажа (0–20): ", 0, 20);
                BaggageItem[] items = new BaggageItem[itemsCount];

                for (int j = 0; j < itemsCount; j++)
                {
                    string itemName = ConsoleInput.ReadRequiredText($"Название единицы {j + 1}: ");
                    items[j] = new BaggageItem(itemName, ReadMass("Масса, кг: "));
                }

                passengers[i] = new Passenger(name, items);
            }

            m = ReadMass("Порог m, кг: ");
        }

        string path = DataPath("passengers.bin");
        FileTasks.FillPassengers(path, passengers);
        Console.WriteLine($"Файл: {path}; XML-сериализация массива структур, затем чтение файла.");

        foreach (Passenger passenger in FileTasks.ReadPassengers(path))
        {
            Console.Write($"{passenger.Name}: {passenger.Items.Length} ед.");

            foreach (BaggageItem item in passenger.Items)
            {
                Console.Write($"; {item.Name} — {item.Mass} кг");
            }

            Console.WriteLine();
        }

        bool found = FileTasks.HasLightSingleBaggage(path, m);
        Console.WriteLine($"Одна единица багажа массой менее {m} кг: {(found ? "Да" : "Нет")}");
    }

    private static void RunTextNumbers(int task, bool manual)
    {
        int count = manual ? ConsoleInput.ReadInt("Количество чисел (1–100): ", 1, 100) : 12;
        int perLine = task == 6 ? 1 : 4;

        if (manual && task == 7)
        {
            perLine = ConsoleInput.ReadInt("Чисел в строке (2–10): ", 2, 10);
        }

        string path = DataPath($"task{task}.txt");
        FileTasks.FillTextNumbers(path, count, perLine, new Random(3));
        Console.WriteLine($"Исходный файл: {path}");
        Console.Write(File.ReadAllText(path));

        if (task == 6)
        {
            Console.WriteLine($"Максимум − минимум = {FileTasks.FindRange(path)}");
        }
        else
        {
            Console.WriteLine($"Минимальный элемент = {FileTasks.FindMinimum(path)}");
        }
    }

    private static void RunTextLines(bool manual)
    {
        string[] lines = { "Багаж готов", "билет куплен", "", "Борис в пути", " Буква после пробела", "Анна дома" };
        char firstCharacter = 'Б';

        if (manual)
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

            firstCharacter = symbol[0];
            int count = ConsoleInput.ReadInt("Количество строк (0–30): ", 0, 30);
            lines = new string[count];

            for (int i = 0; i < count; i++)
            {
                Console.Write($"Строка {i + 1}: ");
                lines[i] = Console.ReadLine() ?? throw new EndOfStreamException();
            }
        }

        string source = DataPath("lines.txt");
        string destination = DataPath("selected_lines.txt");
        File.WriteAllLines(source, lines, Encoding.UTF8);
        FileTasks.FilterLines(source, destination, firstCharacter);
        Console.WriteLine($"Исходный файл: {source}");
        Console.Write(File.ReadAllText(source));
        Console.WriteLine($"Строки на символ «{firstCharacter}», с учётом регистра: {destination}");
        string result = File.ReadAllText(destination);
        Console.WriteLine(result.Length == 0 ? "(пусто)" : result);
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
        while (true)
        {
            int value = ConsoleInput.ReadInt(prompt, int.MinValue, int.MaxValue);

            if (value != 0)
            {
                return value;
            }

            Console.WriteLine("Делитель не должен быть равен нулю.");
        }
    }

    private static decimal ReadMass(string prompt)
    {
        while (true)
        {
            string text = ConsoleInput.ReadRequiredText(prompt).Replace(',', '.');

            if (decimal.TryParse(text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out decimal value) && value > 0)
            {
                return value;
            }

            Console.WriteLine("Введите положительную массу; дробную часть отделите точкой или запятой.");
        }
    }

    private static string DataPath(string name)
    {
        string directory = Path.Combine("labs", "Lab3", "data");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, name);
    }
}
