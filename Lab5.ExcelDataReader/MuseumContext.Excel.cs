using System.Globalization;
using Lab5.ExcelLibrary.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Lab5.ExcelLibrary;

public sealed partial class MuseumContext
{
    public static MuseumContext Load(string path)
    {
        using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using HSSFWorkbook workbook = new HSSFWorkbook(stream);

        List<Exhibit> exhibits = ReadRows(workbook, "Экспонаты", new[] { "ID экспоната", "Название", "Эпоха" })
            .Select(row => new Exhibit(Integer(row, 0), Text(row, 1), Text(row, 2)))
            .ToList();

        List<Visitor> visitors = ReadRows(workbook, "Посетители", new[] { "ID посетителя", "Имя", "Возраст", "Город проживания" })
            .Select(row => new Visitor(Integer(row, 0), Text(row, 1), Integer(row, 2), Text(row, 3)))
            .ToList();

        List<Ticket> tickets = ReadRows(workbook, "Билеты", new[] { "ID билета", "ID экспоната", "ID посетителя", "Дата", "Стоимость" })
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

    private static IEnumerable<IRow> ReadRows(HSSFWorkbook workbook, string name, string[] headers)
    {
        ISheet sheet = workbook.GetSheet(name)
            ?? throw new InvalidDataException($"Отсутствует лист «{name}».");
        IRow header = sheet.GetRow(0)
            ?? throw new InvalidDataException($"Неверные заголовки на листе «{name}».");
        ICell? cell;
        IRow? row;

        for (int column = 0; column < headers.Length; column++)
        {
            cell = header.GetCell(column);
            if (cell is null || cell.CellType != CellType.String || cell.StringCellValue != headers[column])
            {
                throw new InvalidDataException($"Неверные заголовки на листе «{name}».");
            }
        }

        for (int column = headers.Length; column < header.LastCellNum; column++)
        {
            cell = header.GetCell(column);
            if (cell is not null && cell.CellType != CellType.Blank)
            {
                throw new InvalidDataException($"Неверные заголовки на листе «{name}».");
            }
        }

        for (int index = 1; index <= sheet.LastRowNum; index++)
        {
            row = sheet.GetRow(index);
            if (row is not null && row.Cells.Any(item => item.CellType != CellType.Blank))
            {
                yield return row;
            }
        }
    }

    private static string Text(IRow row, int column)
    {
        ICell? cell = row.GetCell(column);
        if (cell is null || cell.CellType != CellType.String)
        {
            throw new InvalidDataException("Ожидается текст в ячейке Excel.");
        }
        return cell.StringCellValue;
    }

    private static decimal Money(IRow row, int column)
    {
        ICell? cell = row.GetCell(column);
        if (cell is null || cell.CellType != CellType.Numeric || !double.IsFinite(cell.NumericCellValue))
        {
            throw new InvalidDataException("Ожидается число в ячейке Excel.");
        }
        return Convert.ToDecimal(cell.NumericCellValue);
    }

    private static int Integer(IRow row, int column)
    {
        decimal value = Money(row, column);
        if (value != decimal.Truncate(value))
        {
            throw new InvalidDataException("ID и возраст должны быть целыми числами.");
        }
        return checked((int)value);
    }

    private static DateTime Date(IRow row, int column)
    {
        ICell? cell = row.GetCell(column);
        if (cell is null || cell.CellType != CellType.Numeric || !DateUtil.IsCellDateFormatted(cell))
        {
            throw new InvalidDataException("Ожидается ячейка Excel с форматом даты.");
        }
        return cell.DateCellValue
            ?? throw new InvalidDataException("Ожидается ячейка Excel с форматом даты.");
    }

    private static void ValidateUnique(IEnumerable<int> ids, string name)
    {
        if (ids.GroupBy(id => id).Any(group => group.Count() > 1))
        {
            throw new InvalidDataException($"Повторяющиеся ID на листе «{name}».");
        }
    }
}
