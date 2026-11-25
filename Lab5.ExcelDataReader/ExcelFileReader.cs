using System.Data;
using System.Text;
using ExcelDataReader;

namespace Lab5.ExcelLibrary;

public static class ExcelFileReader
{
    /// <summary>Читает листы XLS в таблицы. Первая строка сохраняется как строка данных.</summary>
    public static List<DataTable> Read(string path)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using IExcelDataReader reader = ExcelReaderFactory.CreateBinaryReader(stream);
        List<DataTable> sheets = new List<DataTable>();

        do
        {
            DataTable sheet = new DataTable(reader.Name);
            for (int column = 0; column < reader.FieldCount; column++)
            {
                sheet.Columns.Add($"Column{column + 1}", typeof(object));
            }

            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];
                reader.GetValues(values);
                sheet.Rows.Add(values);
            }

            sheets.Add(sheet);
        }
        while (reader.NextResult());

        return sheets;
    }
}
