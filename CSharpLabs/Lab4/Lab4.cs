using System.Text;
using CSharpLabs.Common;

namespace CSharpLabs.Lab4;

internal static class Lab4
{
    public static void Run()
    {
        while (true)
        {
            ConsoleInput.ClearScreen();
            Console.WriteLine("Лабораторная работа №4 — вариант 3");
            Console.WriteLine("1 — Слияние упорядоченных списков");
            Console.WriteLine("2 — Элементы с равными соседями");
            Console.WriteLine("3 — Заказы посетителей кафе");
            Console.WriteLine("4 — Согласные, встречающиеся в одном слове");
            Console.WriteLine("5 — Лучшие участники многоборья");
            Console.WriteLine("6 — Примеры заданий 1–5");
            Console.WriteLine("0 — Назад");
            int choice = ConsoleInput.ReadInt("Выберите пункт: ", 0, 6);

            if (choice == 0)
            {
                return;
            }

            try
            {
                if (choice == 6)
                {
                    for (int task = 1; task <= 5; task++)
                    {
                        RunTask(task, true);
                    }
                }
                else
                {
                    RunTask(choice, false);
                }
            }
            catch (Exception error) when (IsRecoverable(error))
            {
                Console.WriteLine($"Ошибка: {error.Message}");
            }

            ConsoleInput.WaitForContinue();
        }
    }

    private static void RunTask(int task, bool example)
    {
        Console.WriteLine($"\nЗадание {task}, вариант 3");

        switch (task)
        {
            case 1:
                RunSortedLists(example);
                break;
            case 2:
                RunEqualNeighbors(example);
                break;
            case 3:
                RunCafe(example);
                break;
            case 4:
                RunConsonants(example);
                break;
            case 5:
                RunSports(example);
                break;
        }
    }

    private static void RunSortedLists(bool example)
    {
        List<int> first = example ? new List<int> { -2, 1, 1, 5 } : ReadNumbers("L1", true);
        List<int> second = example ? new List<int> { -3, 1, 4, 9 } : ReadNumbers("L2", true);

        PrintGroup("L1", first);
        PrintGroup("L2", second);
        CollectionTasks.MergeSortedLists(first, second);
        PrintGroup("После слияния", first);
    }

    private static void RunEqualNeighbors(bool example)
    {
        LinkedList<int> list = example
            ? new LinkedList<int>(new[] { 1, 2, 1, 2, 1 })
            : new LinkedList<int>(ReadNumbers("L", false));

        PrintGroup("L", list);
        Console.WriteLine($"Количество элементов с равными соседями: {CollectionTasks.CountEqualNeighbors(list)}");
    }

    private static bool IsRecoverable(Exception error)
    {
        return error is not EndOfStreamException
            && (error is ArgumentException or InvalidDataException or IOException or UnauthorizedAccessException);
    }

    private static List<int> ReadNumbers(string name, bool sorted)
    {
        List<int> numbers = new List<int>();

        while (true)
        {
            string? line = ConsoleInput.ReadOptionalText($"{name} через пробел (Enter — пустой список): ");
            numbers.Clear();

            if (line is null)
            {
                return numbers;
            }

            bool valid = true;

            foreach (string token in line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!int.TryParse(token, out int value) || (sorted && numbers.Count > 0 && value < numbers[^1]))
                {
                    valid = false;
                    break;
                }

                numbers.Add(value);
            }

            if (valid)
            {
                return numbers;
            }

            Console.WriteLine(sorted ? "Введите целые числа по возрастанию." : "Введите только целые числа.");
        }
    }

    private static void RunCafe(bool example)
    {
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;
        List<string> menu;
        List<HashSet<string>> orders;

        if (example)
        {
            menu = new List<string> { "Борщ", "Салат", "Компот" };
            orders = new List<HashSet<string>>
            {
                new HashSet<string>(comparer) { "Борщ", "Салат" },
                new HashSet<string>(comparer) { "борщ" }
            };
        }
        else
        {
            menu = ReadCafeMenu(comparer);
            orders = ReadCafeOrders(new HashSet<string>(menu, comparer), comparer);
        }

        PrintGroup("Меню", menu);

        for (int i = 0; i < orders.Count; i++)
        {
            PrintGroup($"Заказ №{i + 1}", orders[i]);
        }

        (List<string> all, List<string> some, List<string> none) = CollectionTasks.ClassifyDishes(menu, orders);
        PrintGroup("Заказывали все", all);
        PrintGroup("Заказывали только некоторые", some);
        PrintGroup("Никто не заказывал", none);
    }

    private static List<string> ReadCafeMenu(StringComparer comparer)
    {
        int dishCount = ConsoleInput.ReadInt("Количество блюд: ", 1, 100);
        List<string> menu = new List<string>();
        HashSet<string> knownDishes = new HashSet<string>(comparer);

        while (menu.Count < dishCount)
        {
            string dish = ConsoleInput.ReadRequiredText($"Блюдо №{menu.Count + 1}: ");

            if (dish.Contains(';') || !knownDishes.Add(dish))
            {
                Console.WriteLine("Введите новое название без точки с запятой.");
                continue;
            }

            menu.Add(dish);
        }

        return menu;
    }

    private static List<HashSet<string>> ReadCafeOrders(HashSet<string> menu, StringComparer comparer)
    {
        int visitorCount = ConsoleInput.ReadInt("Количество посетителей: ", 0, 100);
        List<HashSet<string>> orders = new List<HashSet<string>>();

        while (orders.Count < visitorCount)
        {
            string? line = ConsoleInput.ReadOptionalText($"Заказ №{orders.Count + 1} (блюда через «;», Enter — пустой): ");
            HashSet<string> order = new HashSet<string>(comparer);

            if (line is not null)
            {
                order.UnionWith(line.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
            }

            if (!order.IsSubsetOf(menu))
            {
                Console.WriteLine("Заказывайте только блюда из меню.");
                continue;
            }

            orders.Add(order);
        }

        return orders;
    }

    private static void RunConsonants(bool example)
    {
        if (!example)
        {
            PrintConsonants(ConsoleInput.ReadRequiredText("Путь к текстовому файлу: "));
            return;
        }

        const string sample = "Кукушка, кот и ёж.";
        string path = Path.GetTempFileName();

        try
        {
            File.WriteAllText(path, sample, Encoding.UTF8);
            Console.WriteLine(sample);
            PrintConsonants(path);
        }
        finally
        {
            File.Delete(path);
        }
    }

    private static void PrintConsonants(string path)
    {
        PrintGroup("Согласные ровно в одном слове", CollectionTasks.FindConsonantsInOneWord(path));
    }

    private static void RunSports(bool example)
    {
        string? existingPath = example ? null : ConsoleInput.ReadOptionalText("Путь к файлу результатов (Enter — ввести с клавиатуры): ");

        if (existingPath is not null)
        {
            PrintGroup("Лучшие участники", CollectionTasks.FindBestAthletes(existingPath));
            return;
        }

        List<string> participants = example ? CreateExampleParticipants() : ReadParticipants();

        string path = Path.GetTempFileName();

        try
        {
            CollectionTasks.FillSportsFile(path, participants);

            if (example)
            {
                Console.WriteLine(File.ReadAllText(path));
            }

            PrintGroup("Лучшие участники", CollectionTasks.FindBestAthletes(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    private static List<string> CreateExampleParticipants()
    {
        return new List<string>
        {
            "Петров Сергей 9 9 5 7",
            "Сидорова Анна 8 8 7 5",
            "Иванова Мария 5 8 6 3",
            "Смирнов Павел 4 6 4 8",
            "Орлова Вера 1 2 3 4"
        };
    }

    private static List<string> ReadParticipants()
    {
        int count = ConsoleInput.ReadInt("Количество участников: ", 0, 100);
        List<string> participants = new List<string>();
        Console.WriteLine("Формат строки: Фамилия Имя Балл1 Балл2 Балл3 Балл4");

        for (int i = 0; i < count; i++)
        {
            participants.Add(ConsoleInput.ReadRequiredText($"Участник №{i + 1}: "));
        }

        return participants;
    }

    private static void PrintGroup<T>(string name, IEnumerable<T> values)
    {
        string text = string.Join(", ", values);
        Console.WriteLine($"{name}: {(text.Length == 0 ? "(пусто)" : text)}");
    }
}
