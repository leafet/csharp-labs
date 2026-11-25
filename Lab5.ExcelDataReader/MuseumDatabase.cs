using System.Data;
using System.Globalization;
using Lab5.ExcelLibrary.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Lab5.ExcelLibrary;

/// <summary>Загрузка, просмотр, изменение, LINQ-запросы и сохранение базы музея.</summary>
public sealed class MuseumDatabase
{
    private static readonly DateTime JuneStart = new DateTime(2023, 6, 1);
    private static readonly DateTime JulyStart = new DateTime(2023, 7, 1);

    private readonly List<Exhibit> _exhibits;
    private readonly List<Visitor> _visitors;
    private readonly List<Ticket> _tickets;

    public IReadOnlyList<Exhibit> Exhibits
    {
        get { return _exhibits.AsReadOnly(); }
    }

    public IReadOnlyList<Visitor> Visitors
    {
        get { return _visitors.AsReadOnly(); }
    }

    public IReadOnlyList<Ticket> Tickets
    {
        get { return _tickets.AsReadOnly(); }
    }


    private MuseumDatabase(List<Exhibit> exhibits, List<Visitor> visitors, List<Ticket> tickets)
    {
        _exhibits = exhibits;
        _visitors = visitors;
        _tickets = tickets;
    }

    public static MuseumDatabase Load(string path)
    {
        List<DataTable> tables = ExcelFileReader.Read(path);
        List<Exhibit> exhibits = ReadRows(tables, "Экспонаты", new[] { "ID экспоната", "Название", "Эпоха" })
            .Select(row => new Exhibit(Integer(row, 0), Text(row, 1), Text(row, 2))).ToList();
        List<Visitor> visitors = ReadRows(tables, "Посетители", new[] { "ID посетителя", "Имя", "Возраст", "Город проживания" })
            .Select(row => new Visitor(Integer(row, 0), Text(row, 1), Integer(row, 2), Text(row, 3))).ToList();
        List<Ticket> tickets = ReadRows(tables, "Билеты", new[] { "ID билета", "ID экспоната", "ID посетителя", "Дата", "Стоимость" })
            .Select(row => new Ticket(Integer(row, 0), Integer(row, 1), Integer(row, 2), Date(row, 3), Money(row, 4))).ToList();

        ValidateUnique(exhibits.Select(item => item.Id), "Экспонаты");
        ValidateUnique(visitors.Select(item => item.Id), "Посетители");
        ValidateUnique(tickets.Select(item => item.Id), "Билеты");
        HashSet<int> exhibitIds = exhibits.Select(item => item.Id).ToHashSet();
        HashSet<int> visitorIds = visitors.Select(item => item.Id).ToHashSet();
        if (tickets.Any(ticket => !exhibitIds.Contains(ticket.ExhibitId) || !visitorIds.Contains(ticket.VisitorId)))
        {
            throw new InvalidDataException("Билет ссылается на отсутствующего посетителя или экспонат.");
        }
        return new MuseumDatabase(exhibits, visitors, tickets);
    }

    public void Add(Exhibit exhibit)
    {
        ArgumentNullException.ThrowIfNull(exhibit);
        EnsureNewId(_exhibits.Any(item => item.Id == exhibit.Id));
        _exhibits.Add(exhibit);
    }

    public void Add(Visitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        EnsureNewId(_visitors.Any(item => item.Id == visitor.Id));
        _visitors.Add(visitor);
    }

    public void Add(Ticket ticket)
    {
        ArgumentNullException.ThrowIfNull(ticket);
        EnsureNewId(_tickets.Any(item => item.Id == ticket.Id));
        if (!_exhibits.Any(item => item.Id == ticket.ExhibitId) || !_visitors.Any(item => item.Id == ticket.VisitorId))
        {
            throw new ArgumentException("Сначала добавьте посетителя и экспонат, указанные в билете.");
        }
        _tickets.Add(ticket);
    }

    // Связанные записи защищены от удаления, чтобы сохранить целостность базы.
    public bool DeleteExhibit(int id)
    {
        if (_tickets.Any(ticket => ticket.ExhibitId == id))
        {
            throw new InvalidOperationException("На экспонат есть билеты. Сначала удалите связанные билеты.");
        }
        Exhibit? item = _exhibits.SingleOrDefault(exhibit => exhibit.Id == id);
        return item is not null && _exhibits.Remove(item);
    }

    public bool DeleteVisitor(int id)
    {
        if (_tickets.Any(ticket => ticket.VisitorId == id))
        {
            throw new InvalidOperationException("У посетителя есть билеты. Сначала удалите связанные билеты.");
        }
        Visitor? item = _visitors.SingleOrDefault(visitor => visitor.Id == id);
        return item is not null && _visitors.Remove(item);
    }

    public bool DeleteTicket(int id)
    {
        Ticket? item = _tickets.SingleOrDefault(ticket => ticket.Id == id);
        return item is not null && _tickets.Remove(item);
    }

    public IEnumerable<string> View(int table)
    {
        return table switch
        {
            1 => _exhibits.OrderBy(item => item.Id).Select(item => item.ToString()),
            2 => _visitors.OrderBy(item => item.Id).Select(item => item.ToString()),
            3 => _tickets.OrderBy(item => item.Id).Select(item => item.ToString()),
            _ => throw new ArgumentOutOfRangeException(nameof(table))
        };
    }

    // Запрос 1, одна таблица, перечень экспонатов эпохи Ренессанс.
    public List<Exhibit> RenaissanceExhibits()
    {
        return _exhibits
            .Where(IsRenaissanceExhibit)
            .OrderBy(exhibit => exhibit.Id)
            .ToList();
    }

    // Запрос 2, две таблицы, одно значение. Пример из условия.
    public decimal RenaissanceJuneRevenue()
    {
        return RenaissanceJuneTickets().Sum(ticket => ticket.Price);
    }

    // Запрос 3, три таблицы, перечень посетителей без повторов.
    public List<Visitor> RenaissanceJuneVisitors()
    {
        return RenaissanceJuneTickets()
            .Join(
                _visitors,
                ticket => ticket.VisitorId,
                visitor => visitor.Id,
                (ticket, visitor) => visitor)
            .Distinct()
            .OrderBy(visitor => visitor.Id)
            .ToList();
    }

    // Запрос 4, три таблицы, число билетов совершеннолетних посетителей.
    public int AdultRenaissanceJuneTickets()
    {
        IEnumerable<Visitor> adults = _visitors.Where(visitor => visitor.Age >= 18);

        return RenaissanceJuneTickets()
            .Join(
                adults,
                ticket => ticket.VisitorId,
                visitor => visitor.Id,
                (ticket, visitor) => ticket)
            .Count();
    }

    private IEnumerable<Ticket> RenaissanceJuneTickets()
    {
        IEnumerable<Ticket> juneTickets = _tickets.Where(ticket => ticket.Date >= JuneStart && ticket.Date < JulyStart);
        IEnumerable<Exhibit> renaissanceExhibits = _exhibits.Where(IsRenaissanceExhibit);

        return juneTickets.Join(
            renaissanceExhibits,
            ticket => ticket.ExhibitId,
            exhibit => exhibit.Id,
            (ticket, exhibit) => ticket);
    }

    private static bool IsRenaissanceExhibit(Exhibit exhibit)
    {
        return exhibit.Era.Equals("Ренессанс", StringComparison.OrdinalIgnoreCase);
    }

    public void Save(string path)
    {
        if (!Path.GetExtension(path).Equals(".xls", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Для сохранения выберите файл с расширением .xls.");
        }
        if (new[] { _exhibits.Count, _visitors.Count, _tickets.Count }.Any(count => count > 65535))
        {
            throw new InvalidOperationException("В XLS можно записать до 65535 записей и строку заголовков на листе.");
        }

        using HSSFWorkbook workbook = new HSSFWorkbook();
        ICellStyle dateStyle = workbook.CreateCellStyle();
        dateStyle.DataFormat = workbook.CreateDataFormat().GetFormat("dd.mm.yyyy");
        WriteTable(workbook, "Экспонаты", new[] { "ID экспоната", "Название", "Эпоха" },
            _exhibits.Select(item => new object[] { item.Id, item.Name, item.Era }), dateStyle);
        WriteTable(workbook, "Посетители", new[] { "ID посетителя", "Имя", "Возраст", "Город проживания" },
            _visitors.Select(item => new object[] { item.Id, item.Name, item.Age, item.City }), dateStyle);
        WriteTable(workbook, "Билеты", new[] { "ID билета", "ID экспоната", "ID посетителя", "Дата", "Стоимость" },
            _tickets.Select(item => new object[] { item.Id, item.ExhibitId, item.VisitorId, item.Date, item.Price }), dateStyle);

        string destination = Path.GetFullPath(path);
        string temporary = Path.Combine(Path.GetDirectoryName(destination)!, $".{Guid.NewGuid():N}.tmp");
        try
        {
            using (FileStream stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write))
            {
                workbook.Write(stream, true);
            }
            File.Move(temporary, destination, true);
        }
        finally
        {
            if (File.Exists(temporary))
            {
                File.Delete(temporary);
            }
        }
    }

    private static void WriteTable(HSSFWorkbook workbook, string name, string[] headers,
        IEnumerable<object[]> rows, ICellStyle dateStyle)
    {
        ISheet sheet = workbook.CreateSheet(name);
        IRow header = sheet.CreateRow(0);
        for (int column = 0; column < headers.Length; column++)
        {
            header.CreateCell(column).SetCellValue(headers[column]);
            sheet.SetColumnWidth(column, column == 0 ? 4500 : 6500);
        }
        int index = 1;
        foreach (object[] values in rows)
        {
            IRow row = sheet.CreateRow(index++);
            for (int column = 0; column < values.Length; column++)
            {
                ICell cell = row.CreateCell(column);
                switch (values[column])
                {
                    case string text:
                        cell.SetCellValue(text);
                        break;
                    case DateTime date:
                        cell.SetCellValue(date);
                        cell.CellStyle = dateStyle;
                        break;
                    default:
                        cell.SetCellValue(Convert.ToDouble(values[column], CultureInfo.InvariantCulture));
                        break;
                }
            }
        }
        sheet.CreateFreezePane(0, 1);
    }

    private static IEnumerable<DataRow> ReadRows(List<DataTable> tables, string name, string[] headers)
    {
        DataTable table = tables.SingleOrDefault(item => item.TableName == name)
            ?? throw new InvalidDataException($"Отсутствует лист «{name}».");
        if (table.Columns.Count != headers.Length || table.Rows.Count == 0 ||
            !headers.Select((header, column) => Equals(table.Rows[0][column], header)).All(equal => equal))
        {
            throw new InvalidDataException($"Неверные заголовки на листе «{name}».");
        }
        return table.AsEnumerable().Skip(1).Where(row => row.ItemArray.Any(value => value != DBNull.Value));
    }

    private static string Text(DataRow row, int column)
    {
        return row[column] as string ?? throw new InvalidDataException("Ожидается текст в ячейке Excel.");
    }

    private static decimal Money(DataRow row, int column)
    {
        if (row[column] is not double number || !double.IsFinite(number))
        {
            throw new InvalidDataException("Ожидается число в ячейке Excel.");
        }
        return Convert.ToDecimal(number);
    }

    private static int Integer(DataRow row, int column)
    {
        decimal value = Money(row, column);
        if (value != decimal.Truncate(value))
        {
            throw new InvalidDataException("ID и возраст должны быть целыми числами.");
        }
        return checked((int)value);
    }

    private static DateTime Date(DataRow row, int column)
    {
        return row[column] is DateTime date ? date
            : throw new InvalidDataException("Ожидается ячейка Excel с форматом даты.");
    }

    private static void ValidateUnique(IEnumerable<int> ids, string name)
    {
        if (ids.GroupBy(id => id).Any(group => group.Count() > 1))
        {
            throw new InvalidDataException($"Повторяющиеся ID на листе «{name}».");
        }
    }

    private static void EnsureNewId(bool exists)
    {
        if (exists)
        {
            throw new ArgumentException("Запись с таким ID уже существует.");
        }
    }
}
