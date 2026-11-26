using System.Globalization;
using CSharpLabs.Common;
using Lab5.ExcelLibrary.Models;

namespace Lab5.ExcelLibrary;

internal static class Program
{
    private static int Main(string[] args)
    {
        string path = args.Length == 0 ? Path.Combine(AppContext.BaseDirectory, "LR5-var3.xls") : args[0];
        MuseumContext? database = null;
        bool changed = false;
        int choice;
        string input;
        string output;
        try
        {
            try
            {
                database = MuseumContext.Load(path);
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
                choice = ConsoleInput.ReadInt("Выберите пункт: ", 0, 6);
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
                        input = ConsoleInput.ReadRequiredText("Путь к XLS: ");
                        database = MuseumContext.Load(input);
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
                            AddRecord(database);
                            changed = true;
                            Console.WriteLine("Запись добавлена.");
                            break;
                        case 4:
                            if (Delete(database))
                            {
                                changed = true;
                                Console.WriteLine("Запись удалена.");
                            }
                            else
                            {
                                Console.WriteLine("Запись с таким ID не найдена.");
                            }
                            break;
                        case 5:
                            RunQuery(database);
                            break;
                        case 6:
                            output = ConsoleInput.ReadOptionalText($"Путь сохранения (Enter — {database.FilePath}): ") ?? database.FilePath;
                            if (File.Exists(output) && ConsoleInput.ReadInt("Файл существует. Заменить? 1 — да, 0 — назад: ", 0, 1) == 0)
                            {
                                break;
                            }
                            database.SaveChanges(output);
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
        if (error is EndOfStreamException)
        {
            return false;
        }

        return error is IOException or InvalidDataException or UnauthorizedAccessException or ArgumentException
            or NotSupportedException or InvalidOperationException or OverflowException or FormatException;
    }

    private static int ChooseTable()
    {
        Console.WriteLine("1 — Экспонаты; 2 — Посетители; 3 — Билеты");
        return ConsoleInput.ReadInt("Таблица: ", 1, 3);
    }

    private static void ShowTable(MuseumContext database)
    {
        int table = ChooseTable();
        string[] headers = { "ID | Название | Эпоха", "ID | Имя | Возраст | Город проживания", "ID | ID экспоната | ID посетителя | Дата | Стоимость" };
        List<string> rows = table switch
        {
            1 => database.Exhibits.OrderBy(item => item.Id).Select(item => item.ToString()).ToList(),
            2 => database.Visitors.OrderBy(item => item.Id).Select(item => item.ToString()).ToList(),
            _ => database.Tickets.OrderBy(item => item.Id).Select(item => item.ToString()).ToList()
        };

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

    private static void AddRecord(MuseumContext database)
    {
        int table = ChooseTable();
        int id = ConsoleInput.ReadInt("ID: ", 1, int.MaxValue);
        switch (table)
        {
            case 1:
                Exhibit item = new Exhibit(id, ConsoleInput.ReadRequiredText("Название: "), ConsoleInput.ReadRequiredText("Эпоха: "));
                database.AddExhibit(item);
                break;
            case 2:
                string name = ConsoleInput.ReadRequiredText("Имя: ");
                int age = ConsoleInput.ReadInt("Возраст: ", 0, 150);
                Visitor visitor = new Visitor(id, name, age, ConsoleInput.ReadRequiredText("Город проживания: "));
                database.AddVisitor(visitor);
                break;
            case 3:
                int exhibitId = ConsoleInput.ReadInt("ID экспоната: ", 1, int.MaxValue);
                int visitorId = ConsoleInput.ReadInt("ID посетителя: ", 1, int.MaxValue);
                Ticket ticket = new Ticket(id, exhibitId, visitorId, ReadDate(), ReadPrice());
                database.AddTicket(ticket);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(table));
        }
    }

    private static bool Delete(MuseumContext database)
    {
        int table = ChooseTable();
        int id = ConsoleInput.ReadInt("ID удаляемой записи: ", 1, int.MaxValue);
        return table switch
        {
            1 => database.RemoveExhibit(id),
            2 => database.RemoveVisitor(id),
            _ => database.RemoveTicket(id)
        };
    }

    private static void RunQuery(MuseumContext database)
    {
        Console.WriteLine("1 — Перечень экспонатов эпохи Ренессанс (1 таблица)");
        Console.WriteLine("2 — Выручка от билетов на Ренессанс за июнь 2023 года (2 таблицы)");
        Console.WriteLine("3 — Посетители экспонатов Ренессанса в июне 2023 года (3 таблицы)");
        Console.WriteLine("4 — Число билетов совершеннолетних на Ренессанс за июнь 2023 года (3 таблицы)");
        int query = ConsoleInput.ReadInt("Запрос: ", 1, 4);
        switch (query)
        {
            case 1:
                PrintList(MuseumQueries.RenaissanceExhibits(database));
                break;
            case 2:
                Console.WriteLine($"Выручка: {MuseumQueries.RenaissanceRevenue(database):F2} руб.");
                break;
            case 3:
                PrintList(MuseumQueries.RenaissanceVisitors(database));
                break;
            case 4:
                Console.WriteLine($"Количество билетов: {MuseumQueries.AdultTicketCount(database)}");
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
        string text;
        DateTime date;
        while (true)
        {
            text = ConsoleInput.ReadRequiredText("Дата (дд.мм.гггг): ");
            if (DateTime.TryParseExact(text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                && date >= new DateTime(1900, 1, 1))
            {
                return date;
            }
            Console.WriteLine("Введите существующую дату не ранее 01.01.1900.");
        }
    }

    private static decimal ReadPrice()
    {
        string text;
        decimal price;
        while (true)
        {
            text = ConsoleInput.ReadRequiredText("Стоимость, руб.: ").Replace(',', '.');
            if (decimal.TryParse(text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price)
                && price >= 0 && price <= 1_000_000_000m && decimal.Round(price, 2) == price)
            {
                return price;
            }
            Console.WriteLine("Введите стоимость от 0 до 1 000 000 000 с точностью до копеек.");
        }
    }
}
