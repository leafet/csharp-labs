using System.Data;
using System.Globalization;
using System.Text;
using ExcelDataReader;
using Lab5.ExcelLibrary.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Lab5.ExcelLibrary;

public sealed partial class MuseumContext
{
    public static MuseumContext Load(string path)
    {
        List<DataTable> tables = ReadTables(path);

        List<Exhibit> exhibits = ReadRows(tables, "Экспонаты", new[] { "ID экспоната", "Название", "Эпоха" })
            .Select(row => new Exhibit(Integer(row, 0), Text(row, 1), Text(row, 2)))
            .ToList();

        List<Visitor> visitors = ReadRows(tables, "Посетители", new[] { "ID посетителя", "Имя", "Возраст", "Город проживания" })
            .Select(row => new Visitor(Integer(row, 0), Text(row, 1), Integer(row, 2), Text(row, 3)))
            .ToList();

        List<Ticket> tickets = ReadRows(tables, "Билеты", new[] { "ID билета", "ID экспоната", "ID посетителя", "Дата", "Стоимость" })
            .Select(row => new Ticket(Integer(row, 0), Integer(row, 1), Integer(row, 2), Date(row, 3), Money(row, 4)))
            .ToList();

        ValidateUnique(exhibits.Select(item => item.Id), "Экспонаты");
        ValidateUnique(visitors.Select(item => item.Id), "Посетители");
        ValidateUnique(tickets.Select(item => item.Id), "Билеты");

        HashSet<int> exhibitIds = exhibits.Select(item => item.Id).ToHashSet();
        HashSet<int> visitorIds = visitors.Select(item => item.Id).ToHashSet();

        if (tickets.Any(ticket => !exhibitIds.Contains(ticket.ExhibitId) || !visitorIds.Contains(ticket.VisitorId)))
        {
            throw new InvalidDataException("Билет ссылается на отсутствующего посетителя или экспонат.");
        }

        return new MuseumContext(path, exhibits, visitors, tickets);
    }

    public void SaveChanges(string? path = null)
    {
        path ??= FilePath;

        if (!Path.GetExtension(path).Equals(".xls", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Для сохранения выберите файл с расширением .xls.");
        }

        if (new[] { Exhibits.Count, Visitors.Count, Tickets.Count }.Any(count => count > 65535))
        {
            throw new InvalidOperationException("В XLS можно записать до 65535 записей и строку заголовков на листе.");
        }

        using HSSFWorkbook workbook = new HSSFWorkbook();
        ICellStyle dateStyle = workbook.CreateCellStyle();
        dateStyle.DataFormat = workbook.CreateDataFormat().GetFormat("dd.mm.yyyy");

        WriteTable(workbook, "Экспонаты", new[] { "ID экспоната", "Название", "Эпоха" },
            Exhibits.Select(item => new object[] { item.Id, item.Name, item.Era }), dateStyle);

        WriteTable(workbook, "Посетители", new[] { "ID посетителя", "Имя", "Возраст", "Город проживания" },
            Visitors.Select(item => new object[] { item.Id, item.Name, item.Age, item.City }), dateStyle);

        WriteTable(workbook, "Билеты", new[] { "ID билета", "ID экспоната", "ID посетителя", "Дата", "Стоимость" },
            Tickets.Select(item => new object[] { item.Id, item.ExhibitId, item.VisitorId, item.Date, item.Price }), dateStyle);

        string destination = Path.GetFullPath(path);
        string temporary = Path.Combine(Path.GetDirectoryName(destination)!, $".{Guid.NewGuid():N}.tmp");

        try
        {
            using (FileStream stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write))
            {
                workbook.Write(stream, true);
            }

            File.Move(temporary, destination, true);
            FilePath = destination;
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
        IRow row;
        ICell cell;
        object value;
        foreach (object[] values in rows)
        {
            row = sheet.CreateRow(index++);
            for (int column = 0; column < values.Length; column++)
            {
                cell = row.CreateCell(column);
                value = values[column];
                if (value is string)
                {
                    cell.SetCellValue((string)value);
                }
                else if (value is DateTime)
                {
                    cell.SetCellValue((DateTime)value);
                    cell.CellStyle = dateStyle;
                }
                else
                {
                    cell.SetCellValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
                }
            }
        }

        sheet.CreateFreezePane(0, 1);
    }

    private static List<DataTable> ReadTables(string path)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using IExcelDataReader reader = ExcelReaderFactory.CreateBinaryReader(stream);
        List<DataTable> tables = new List<DataTable>();
        DataTable table;
        object[] values;

        do
        {
            table = new DataTable(reader.Name);
            for (int column = 0; column < reader.FieldCount; column++)
            {
                table.Columns.Add($"Column{column + 1}", typeof(object));
            }

            while (reader.Read())
            {
                values = new object[reader.FieldCount];
                reader.GetValues(values);
                table.Rows.Add(values);
            }

            tables.Add(table);
        }
        while (reader.NextResult());

        return tables;
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
}
