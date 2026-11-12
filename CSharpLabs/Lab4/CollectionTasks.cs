using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharpLabs.Lab4;

/// <summary>Решения заданий 1–5 лабораторной работы №4, вариант 3.</summary>
public static class CollectionTasks
{
    private const string RussianConsonants = "бвгджзйклмнпрстфхцчшщ";
    private const int MaximumTotalScore = 40;

    public static void MergeSortedLists(List<int> first, List<int> second)
    {
        if (!IsSorted(first) || !IsSorted(second))
        {
            throw new ArgumentException("Оба списка должны быть упорядочены по возрастанию.");
        }

        List<int> merged = new List<int>(first.Count + second.Count);
        int firstIndex = 0;
        int secondIndex = 0;

        while (firstIndex < first.Count && secondIndex < second.Count)
        {
            if (first[firstIndex] <= second[secondIndex])
            {
                merged.Add(first[firstIndex]);
                firstIndex++;
            }
            else
            {
                merged.Add(second[secondIndex]);
                secondIndex++;
            }
        }

        while (firstIndex < first.Count)
        {
            merged.Add(first[firstIndex]);
            firstIndex++;
        }

        while (secondIndex < second.Count)
        {
            merged.Add(second[secondIndex]);
            secondIndex++;
        }

        first.Clear();
        first.AddRange(merged);
    }

    public static int CountEqualNeighbors(LinkedList<int> list)
    {
        int count = 0;
        LinkedListNode<int>? node = list.First?.Next;

        while (node is not null && node.Next is not null)
        {
            if (node.Previous!.Value == node.Next.Value)
            {
                count++;
            }

            node = node.Next;
        }

        return count;
    }

    public static (List<string> All, List<string> Some, List<string> None) ClassifyDishes(
        IReadOnlyList<string> menu, IReadOnlyList<HashSet<string>> orders)
    {
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;
        HashSet<string> orderedByAll = new HashSet<string>(menu, comparer);
        HashSet<string> orderedByAnyone = new HashSet<string>(comparer);
        List<string> all = new List<string>();
        List<string> some = new List<string>();
        List<string> none = new List<string>();

        foreach (HashSet<string> order in orders)
        {
            orderedByAll.IntersectWith(order);
            orderedByAnyone.UnionWith(order);
        }

        if (orders.Count == 0)
        {
            orderedByAll.Clear();
        }

        HashSet<string> orderedBySome = new HashSet<string>(orderedByAnyone, comparer);
        orderedBySome.ExceptWith(orderedByAll);

        HashSet<string> orderedByNone = new HashSet<string>(menu, comparer);
        orderedByNone.ExceptWith(orderedByAnyone);

        HashSet<string> seen = new HashSet<string>(comparer);

        foreach (string dish in menu)
        {
            if (!seen.Add(dish))
            {
                continue;
            }

            if (orderedByAll.Contains(dish))
            {
                all.Add(dish);
            }
            else if (orderedBySome.Contains(dish))
            {
                some.Add(dish);
            }
            else if (orderedByNone.Contains(dish))
            {
                none.Add(dish);
            }
        }

        return (all, some, none);
    }

    public static string FindConsonantsInOneWord(string path)
    {
        using StreamReader reader = OpenInputFile(path);
        HashSet<char> seenInAnyWord = new HashSet<char>();
        HashSet<char> seenInSeveralWords = new HashSet<char>();
        HashSet<char> lettersInCurrentWord = new HashSet<char>();
        StringBuilder result = new StringBuilder();

        string? line;

        while ((line = reader.ReadLine()) is not null)
        {
            foreach (Match word in Regex.Matches(line.ToLowerInvariant(), "[а-яё]+"))
            {
                lettersInCurrentWord.Clear();

                foreach (char letter in word.Value)
                {
                    if (RussianConsonants.Contains(letter))
                    {
                        lettersInCurrentWord.Add(letter);
                    }
                }

                foreach (char letter in lettersInCurrentWord)
                {
                    if (!seenInAnyWord.Add(letter))
                    {
                        seenInSeveralWords.Add(letter);
                    }
                }
            }
        }

        seenInAnyWord.ExceptWith(seenInSeveralWords);

        foreach (char letter in RussianConsonants)
        {
            if (seenInAnyWord.Contains(letter))
            {
                result.Append(letter);
            }
        }

        return result.ToString();
    }

    public static void FillSportsFile(string path, IReadOnlyList<string> participants)
    {
        ValidateFilePath(path);
        ValidateParticipantCount(participants.Count);

        foreach (string participant in participants)
        {
            ParseParticipant(participant);
        }

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine(participants.Count.ToString(CultureInfo.InvariantCulture));

        foreach (string participant in participants)
        {
            writer.WriteLine(participant);
        }
    }

    public static List<string> FindBestAthletes(string path)
    {
        using StreamReader reader = OpenInputFile(path);
        Dictionary<int, List<string>> standings = new Dictionary<int, List<string>>();
        List<string> winners = new List<string>();
        string? line;

        if (!int.TryParse(reader.ReadLine(), NumberStyles.None, CultureInfo.InvariantCulture, out int count))
        {
            throw new InvalidDataException("Первая строка файла должна содержать количество участников.");
        }

        ValidateParticipantCount(count);

        for (int i = 0; i < count; i++)
        {
            line = reader.ReadLine() ?? throw new InvalidDataException($"Отсутствует строка участника №{i + 1}.");
            (string name, int total) = ParseParticipant(line);

            if (!standings.TryGetValue(total, out List<string>? tied))
            {
                tied = new List<string>();
                standings.Add(total, tied);
            }

            tied.Add(name);
        }

        while ((line = reader.ReadLine()) is not null)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                throw new InvalidDataException("В файле больше участников, чем указано в первой строке.");
            }
        }

        // Сумма четырёх оценок лежит в диапазоне 0–40: сортировка не нужна.
        for (int score = MaximumTotalScore; score >= 0 && winners.Count < 3; score--)
        {
            if (standings.TryGetValue(score, out List<string>? tied))
            {
                winners.AddRange(tied);
            }
        }

        return winners;
    }

    private static StreamReader OpenInputFile(string path)
    {
        ValidateFilePath(path);

        try
        {
            return new StreamReader(path, Encoding.UTF8);
        }
        catch (FileNotFoundException error)
        {
            throw new FileNotFoundException($"Входной файл «{path}» не найден.", path, error);
        }
        catch (DirectoryNotFoundException error)
        {
            throw new DirectoryNotFoundException($"Не найдена папка входного файла «{path}».", error);
        }
        catch (UnauthorizedAccessException error)
        {
            throw new UnauthorizedAccessException($"Нет доступа к чтению файла «{path}».", error);
        }
    }

    private static void ValidateFilePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Укажите путь к файлу.", nameof(path));
        }

        if (Directory.Exists(path))
        {
            throw new ArgumentException($"Путь «{path}» указывает на папку. Укажите файл.", nameof(path));
        }
    }

    private static bool IsSorted(List<int> values)
    {
        for (int i = 1; i < values.Count; i++)
        {
            if (values[i] < values[i - 1])
            {
                return false;
            }
        }

        return true;
    }

    private static void ValidateParticipantCount(int count)
    {
        if (count < 0 || count > 100)
        {
            throw new InvalidDataException("Количество участников должно быть от 0 до 100.");
        }
    }

    private static (string Name, int Total) ParseParticipant(string line)
    {
        string[] fields = line.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        int total = 0;
        int score = 0;

        if (fields.Length != 6 || fields[0].Length > 20 || fields[1].Length > 15)
        {
            throw new InvalidDataException("Ожидаются фамилия (до 20 символов), имя (до 15) и четыре балла.");
        }

        for (int i = 2; i < fields.Length; i++)
        {
            if (!int.TryParse(fields[i], NumberStyles.None, CultureInfo.InvariantCulture, out score) || score > 10)
            {
                throw new InvalidDataException("Каждый балл должен быть целым числом от 0 до 10.");
            }

            total += score;
        }

        return ($"{fields[0]} {fields[1]}", total);
    }
}
