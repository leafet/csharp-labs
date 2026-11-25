using System.Globalization;
using CSharpLabs.Common;
using ExcelDataReader.Exceptions;
using Lab5.ExcelLibrary.Models;

namespace Lab5.ExcelLibrary;

internal static class Program
{
    private static int Main(string[] args)
    {
        string path = args.Length == 0 ? Path.Combine(AppContext.BaseDirectory, "LR5-var3.xls") : args[0];
        MuseumDatabase? database = null;
        bool changed = false;
        try
        {
            try
            {
                database = MuseumDatabase.Load(path);
                Console.WriteLine($"Загружен файл {path}");
            }
            catch (Exception error) when (IsDataError(error))
            {
                Console.WriteLine($"Не удалось загрузить базу. {error.Message}");
            }

            while (true)
            {
                Console.WriteLine("\nЛР5 — вариант 3. Музей");
                Console.WriteLine("1 — Прочитать базу из XLS");
                Console.WriteLine("2 — Просмотреть таблицу");
                Console.WriteLine("3 — Добавить запись");
                Console.WriteLine("4 — Удалить запись по ID");
                Console.WriteLine("5 — Выполнить LINQ-запросы");
                Console.WriteLine("6 — Сохранить базу в XLS");
                Console.WriteLine(changed ? "Есть несохранённые изменения." : "");
                Console.WriteLine("0 — Выход");
                int choice = ConsoleInput.ReadInt("Выберите пункт: ", 0, 6);
                try
                {
                    if (choice == 0)
                    {
                        if (!changed || ConsoleInput.ReadInt("Выйти без сохранения? 1 — да, 0 — назад: ", 0, 1) == 1)
                        {
                            return 0;
                        }
                        continue;
                    }

                    if (choice == 1)
                    {
                        if (changed && ConsoleInput.ReadInt("Заменить несохранённые данные? 1 — да, 0 — назад: ", 0, 1) == 0)
                        {
                            continue;
                        }
                        string input = ConsoleInput.ReadRequiredText("Путь к XLS: ");
                        MuseumDatabase loaded = MuseumDatabase.Load(input);
                        database = loaded;
                        path = input;
                        changed = false;
                        Console.WriteLine("База загружена.");
                        continue;
                    }

                    if (database is null)
                    {
                        Console.WriteLine("Сначала загрузите базу из файла.");
                        continue;
                    }
                    switch (choice)
                    {
                        case 2:
                            ShowTable(database);
                            break;
                        case 3:
                            Add(database);
                            changed = true;
                            Console.WriteLine("Запись добавлена.");
                            break;
                        case 4:
                            bool removed = Delete(database);
                            changed |= removed;
                            Console.WriteLine(removed ? "Запись удалена." : "Запись с таким ID не найдена.");
                            break;
                        case 5:
                            RunQuery(database);
                            break;
                        case 6:
                            string output = ConsoleInput.ReadOptionalText($"Путь сохранения (Enter — {path}): ") ?? path;
                            if (File.Exists(output) && ConsoleInput.ReadInt("Файл существует. Заменить? 1 — да, 0 — назад: ", 0, 1) == 0)
                            {
                                break;
                            }
                            database.Save(output);
                            path = output;
                            changed = false;
                            Console.WriteLine($"База сохранена: {Path.GetFullPath(output)}");
                            break;
                    }
                }
                catch (Exception error) when (IsDataError(error))
                {
                    Console.WriteLine($"Ошибка: {error.Message}");
                }
            }
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine(changed ? "\nВвод завершён. Изменения остались несохранёнными." : "\nВвод завершён.");
            return 0;
        }
    }

    private static bool IsDataError(Exception error)
    {
        return error is IOException or InvalidDataException or UnauthorizedAccessException or ArgumentException
            or NotSupportedException or ExcelReaderException or InvalidOperationException or OverflowException or FormatException
            && error is not EndOfStreamException;
    }

    private static int ChooseTable()
    {
        Console.WriteLine("1 — Экспонаты; 2 — Посетители; 3 — Билеты");
        return ConsoleInput.ReadInt("Таблица: ", 1, 3);
    }

    private static void ShowTable(MuseumDatabase database)
    {
        int table = ChooseTable();
        string[] headers = { "ID | Название | Эпоха", "ID | Имя | Возраст | Город проживания", "ID | ID экспоната | ID посетителя | Дата | Стоимость" };
        List<string> rows = database.View(table).ToList();
        Console.WriteLine($"Записей: {rows.Count}");
        if (rows.Count == 0)
        {
            return;
        }
        int pages = (rows.Count + 19) / 20;
        int page = ConsoleInput.ReadInt($"Страница (1–{pages}): ", 1, pages);
        Console.WriteLine(headers[table - 1]);
        foreach (string row in rows.Skip((page - 1) * 20).Take(20))
        {
            Console.WriteLine(row);
        }
    }

    private static void Add(MuseumDatabase database)
    {
        int table = ChooseTable();
        int id = ConsoleInput.ReadInt("ID: ", 1, int.MaxValue);
        switch (table)
        {
            case 1:
                database.Add(new Exhibit(id, ConsoleInput.ReadRequiredText("Название: "), ConsoleInput.ReadRequiredText("Эпоха: ")));
                break;
            case 2:
                string name = ConsoleInput.ReadRequiredText("Имя: ");
                int age = ConsoleInput.ReadInt("Возраст: ", 0, 150);
                database.Add(new Visitor(id, name, age, ConsoleInput.ReadRequiredText("Город проживания: ")));
                break;
            case 3:
                int exhibit = ConsoleInput.ReadInt("ID экспоната: ", 1, int.MaxValue);
                int visitor = ConsoleInput.ReadInt("ID посетителя: ", 1, int.MaxValue);
                database.Add(new Ticket(id, exhibit, visitor, ReadDate(), ReadPrice()));
                break;
        }
    }

    private static bool Delete(MuseumDatabase database)
    {
        int table = ChooseTable();
        int id = ConsoleInput.ReadInt("ID удаляемой записи: ", 1, int.MaxValue);
        return table switch
        {
            1 => database.DeleteExhibit(id),
            2 => database.DeleteVisitor(id),
            _ => database.DeleteTicket(id)
        };
    }

    private static void RunQuery(MuseumDatabase database)
    {
        Console.WriteLine("1 — Перечень экспонатов эпохи Ренессанс (1 таблица)");
        Console.WriteLine("2 — Выручка от билетов на Ренессанс за июнь 2023 года (2 таблицы)");
        Console.WriteLine("3 — Посетители экспонатов Ренессанса в июне 2023 года (3 таблицы)");
        Console.WriteLine("4 — Число билетов совершеннолетних на Ренессанс за июнь 2023 года (3 таблицы)");
        int query = ConsoleInput.ReadInt("Запрос: ", 1, 4);
        switch (query)
        {
            case 1:
                PrintList(database.RenaissanceExhibits());
                break;
            case 2:
                Console.WriteLine($"Выручка: {database.RenaissanceJuneRevenue():F2} руб.");
                break;
            case 3:
                PrintList(database.RenaissanceJuneVisitors());
                break;
            case 4:
                Console.WriteLine($"Количество билетов: {database.AdultRenaissanceJuneTickets()}");
                break;
        }
    }

    private static void PrintList<T>(List<T> items)
    {
        foreach (T item in items)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine($"Найдено записей: {items.Count}");
    }

    private static DateTime ReadDate()
    {
        while (true)
        {
            string text = ConsoleInput.ReadRequiredText("Дата (дд.мм.гггг): ");
            if (DateTime.TryParseExact(text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date)
                && date >= new DateTime(1900, 1, 1))
            {
                return date;
            }
            Console.WriteLine("Введите существующую дату не ранее 01.01.1900.");
        }
    }

    private static decimal ReadPrice()
    {
        while (true)
        {
            string text = ConsoleInput.ReadRequiredText("Стоимость, руб.: ").Replace(',', '.');
            if (decimal.TryParse(text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal price)
                && price >= 0 && price <= 1_000_000_000m && decimal.Round(price, 2) == price)
            {
                return price;
            }
            Console.WriteLine("Введите стоимость от 0 до 1 000 000 000 с точностью до копеек.");
        }
    }
}
