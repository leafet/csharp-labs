using CSharpLabs.Common;
using CSharpLabs.Lab3.Common;
using CSharpLabs.Lab3.Tasks;

namespace CSharpLabs.Lab3;

public class Lab3
{
    public void Run()
    {
        int choice;
        bool demonstration;

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
            Console.WriteLine("9 — Демонстрационные примеры с готовыми данными");
            Console.WriteLine("0 — Назад");
            choice = ConsoleInput.ReadInt("Выберите задание: ", 0, 9);

            if (choice == 0)
            {
                return;
            }

            demonstration = choice == 9;

            if (demonstration)
            {
                choice = ConsoleInput.ReadInt("Номер задания (1–8): ", 1, 8);
            }

            RunTaskSafely(choice, demonstration);

            ConsoleInput.WaitForContinue();
        }
    }

    private static void RunTaskSafely(int choice, bool demonstration)
    {
        try
        {
            RunTask(choice, demonstration);
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
    }

    private static void RunTask(int choice, bool demonstration)
    {
        Console.WriteLine($"Задание {choice}, вариант 3");

        if (!demonstration)
        {
            Lab3Input.Run(choice);
        }
        else
        {
            Console.WriteLine("Демонстрационный пример: используются готовые данные.");

            switch (choice)
            {
                case 1:
                    Task1.RunExample();
                    break;
                case 2:
                    Task2.RunExample();
                    break;
                case 3:
                    Task3.RunExample();
                    break;
                case 4:
                    Task4.RunExample();
                    break;
                case 5:
                    Task5.RunExample();
                    break;
                case 6:
                    Task6.RunExample();
                    break;
                case 7:
                    Task7.RunExample();
                    break;
                case 8:
                    Task8.RunExample();
                    break;
            }
        }
    }
}
