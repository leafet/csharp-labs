using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace CSharpLabs.Lab3;

/// <summary>Статические методы заданий 4–8 и заполнения исходных файлов.</summary>
public static class FileTasks
{
    /// <summary>Записывает случайные Int32 в бинарный файл.</summary>
    public static void FillBinaryNumbers(string path, int count, Random random)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentNullException.ThrowIfNull(random);
        using BinaryWriter writer = new BinaryWriter(File.Create(path));

        for (int i = 0; i < count; i++)
        {
            writer.Write(random.Next(-100, 101));
        }
    }

    /// <summary>Сохраняет числа, кратные m и не кратные n, в исходном порядке.</summary>
    public static void FilterBinaryNumbers(string source, string destination, int m, int n)
    {
        ValidateDifferentPaths(source, destination);

        if (m == 0 || n == 0)
        {
            throw new ArgumentException("Делители m и n не должны быть равны нулю.");
        }

        using BinaryReader reader = new BinaryReader(File.OpenRead(source));

        if (reader.BaseStream.Length % sizeof(int) != 0)
        {
            throw new InvalidDataException("Бинарный файл должен содержать целые Int32.");
        }

        using BinaryWriter writer = new BinaryWriter(File.Create(destination));

        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            int value = reader.ReadInt32();

            // Расширение до long исключает переполнение int.MinValue % -1.
            if ((long)value % m == 0 && (long)value % n != 0)
            {
                writer.Write(value);
            }
        }
    }

    /// <summary>Показывает числовой бинарный файл в читаемом виде.</summary>
    public static string ReadBinaryNumbers(string path)
    {
        using BinaryReader reader = new BinaryReader(File.OpenRead(path));
        StringBuilder result = new StringBuilder();

        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            if (result.Length > 0)
            {
                result.Append(' ');
            }

            result.Append(reader.ReadInt32());
        }

        return result.Length == 0 ? "(пусто)" : result.ToString();
    }

    /// <summary>XML-сериализует структуры и записывает XML в бинарный контейнер.</summary>
    public static void FillPassengers(string path, Passenger[] passengers)
    {
        ArgumentNullException.ThrowIfNull(passengers);
        XmlSerializer serializer = new XmlSerializer(typeof(Passenger[]));
        using MemoryStream xml = new MemoryStream();
        serializer.Serialize(xml, passengers);
        byte[] data = xml.ToArray();
        using BinaryWriter writer = new BinaryWriter(File.Create(path));
        writer.Write(data.Length);
        writer.Write(data);
    }

    /// <summary>Читает бинарный контейнер и десериализует XML в массив структур.</summary>
    public static Passenger[] ReadPassengers(string path)
    {
        using BinaryReader reader = new BinaryReader(File.OpenRead(path));
        int length = reader.ReadInt32();

        if (length < 0 || length != reader.BaseStream.Length - sizeof(int))
        {
            throw new InvalidDataException("Повреждена длина XML в бинарном файле.");
        }

        using MemoryStream xml = new MemoryStream(reader.ReadBytes(length));
        XmlSerializer serializer = new XmlSerializer(typeof(Passenger[]));
        return (Passenger[])(serializer.Deserialize(xml)
            ?? throw new InvalidDataException("В файле нет массива пассажиров."));
    }

    /// <summary>Проверяет наличие пассажира с одной единицей багажа массой строго меньше m.</summary>
    public static bool HasLightSingleBaggage(string path, decimal m)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(m);
        Passenger[] passengers = ReadPassengers(path);

        foreach (Passenger passenger in passengers)
        {
            if (passenger.Items.Length == 1 && passenger.Items[0].Mass < m)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Заполняет текстовый файл случайными числами, по указанному количеству в строке.</summary>
    public static void FillTextNumbers(string path, int count, int perLine, Random random)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(perLine);
        ArgumentNullException.ThrowIfNull(random);
        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);

        for (int i = 0; i < count; i++)
        {
            writer.Write(random.Next(-100, 101).ToString(CultureInfo.InvariantCulture));

            if ((i + 1) % perLine == 0 || i == count - 1)
            {
                writer.WriteLine();
            }
            else
            {
                writer.Write(' ');
            }
        }
    }

    /// <summary>Находит разность максимального и минимального чисел, записанных по одному в строке.</summary>
    public static long FindRange(string path)
    {
        using StreamReader reader = new StreamReader(path);
        string? line = reader.ReadLine();

        if (line is null)
        {
            throw new InvalidDataException("В файле нет чисел.");
        }

        int minimum = int.Parse(line, CultureInfo.InvariantCulture);
        int maximum = minimum;

        while ((line = reader.ReadLine()) is not null)
        {
            int value = int.Parse(line, CultureInfo.InvariantCulture);
            minimum = Math.Min(minimum, value);
            maximum = Math.Max(maximum, value);
        }

        return (long)maximum - minimum;
    }

    /// <summary>Находит минимум среди чисел, разделённых пробельными символами.</summary>
    public static int FindMinimum(string path)
    {
        int minimum = int.MaxValue;
        bool found = false;

        foreach (string line in File.ReadLines(path))
        {
            string[] tokens = line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

            foreach (string token in tokens)
            {
                minimum = Math.Min(minimum, int.Parse(token, CultureInfo.InvariantCulture));
                found = true;
            }
        }

        if (!found)
        {
            throw new InvalidDataException("В файле нет чисел.");
        }

        return minimum;
    }

    /// <summary>Переписывает строки, начинающиеся с заданного символа, с учётом регистра.</summary>
    public static void FilterLines(string source, string destination, char firstCharacter)
    {
        ValidateDifferentPaths(source, destination);
        using StreamReader reader = new StreamReader(source);
        using StreamWriter writer = new StreamWriter(destination, false, Encoding.UTF8);
        string? line;

        while ((line = reader.ReadLine()) is not null)
        {
            if (line.Length > 0 && line[0] == firstCharacter)
            {
                writer.WriteLine(line);
            }
        }
    }

    private static void ValidateDifferentPaths(string source, string destination)
    {
        if (string.Equals(Path.GetFullPath(source), Path.GetFullPath(destination),
            StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Исходный и выходной файлы должны различаться.");
        }
    }
}
