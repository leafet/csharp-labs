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
            Console.WriteLine("0 — Назад");
            int choice = ConsoleInput.ReadInt("Выберите пункт: ", 0, 5);

            if (choice == 0)
            {
                return;
            }

            try
            {
                RunTask(choice);
            }
            catch (Exception error) when (IsRecoverable(error))
            {
                Console.WriteLine($"Ошибка: {error.Message}");
            }

            ConsoleInput.WaitForContinue();
        }
    }

    private static void RunTask(int task)
    {
        Console.WriteLine($"\nЗадание {task}, вариант 3");

        switch (task)
        {
            case 1:
                RunSortedLists();
                break;
            case 2:
                RunEqualNeighbors();
                break;
            case 3:
                RunCafe();
                break;
            case 4:
                RunConsonants();
                break;
            case 5:
                RunSports();
                break;
        }
    }

    private static void RunSortedLists()
    {
        List<int> first = ReadNumbers("L1", true);
        List<int> second = ReadNumbers("L2", true);

        PrintGroup("L1", first);
        PrintGroup("L2", second);
        CollectionTasks.MergeSortedLists(first, second);
        PrintGroup("После слияния", first);
    }

    private static void RunEqualNeighbors()
    {
        LinkedList<int> list = new LinkedList<int>(ReadNumbers("L", false));

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

    private static void RunCafe()
    {
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;
        List<string> menu = ReadCafeMenu(comparer);
        List<HashSet<string>> orders = ReadCafeOrders(new HashSet<string>(menu, comparer), comparer);

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

    private static void RunConsonants()
    {
        string path = ConsoleInput.ReadRequiredText("Путь к текстовому файлу: ");
        PrintConsonants(path);
    }

    private static void PrintConsonants(string path)
    {
        PrintGroup("Согласные ровно в одном слове", CollectionTasks.FindConsonantsInOneWord(path));
    }

    private static void RunSports()
    {
        string? existingPath = ConsoleInput.ReadOptionalText("Путь к файлу результатов (Enter — ввести с клавиатуры): ");

        if (existingPath is not null)
        {
            PrintGroup("Лучшие участники", CollectionTasks.FindBestAthletes(existingPath));
            return;
        }

        List<string> participants = ReadParticipants();

        string path = Path.GetTempFileName();

        try
        {
            CollectionTasks.FillSportsFile(path, participants);

            PrintGroup("Лучшие участники", CollectionTasks.FindBestAthletes(path));
        }
        finally
        {
            File.Delete(path);
        }
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
