using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharpLabs.Lab4;

/// <summary>Решения заданий 1–5 лабораторной работы №4, вариант 3.</summary>
public static class CollectionTasks
{
    private const string RussianConsonants = "бвгджзйклмнпрстфхцчшщ";

    public static void MergeSortedLists(List<int> first, List<int> second)
    {
        if (!IsSorted(first) || !IsSorted(second))
        {
            throw new ArgumentException("Оба списка должны быть упорядочены по возрастанию.");
        }

        List<int> merged = new List<int>(first.Count + second.Count);
        int i = 0;
        int j = 0;

        while (i < first.Count && j < second.Count)
        {
            if (first[i] <= second[j])
            {
                merged.Add(first[i++]);
            }
            else
            {
                merged.Add(second[j++]);
            }
        }

        while (i < first.Count)
        {
            merged.Add(first[i++]);
        }

        while (j < second.Count)
        {
            merged.Add(second[j++]);
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
        HashSet<string> allOrdered = new HashSet<string>(menu, comparer);
        HashSet<string> anyOrdered = new HashSet<string>(comparer);
        HashSet<string> seen = new HashSet<string>(comparer);
        List<string> all = new List<string>();
        List<string> some = new List<string>();
        List<string> none = new List<string>();

        foreach (HashSet<string> order in orders)
        {
            allOrdered.IntersectWith(order);
            anyOrdered.UnionWith(order);
        }

        foreach (string dish in menu)
        {
            if (!seen.Add(dish))
            {
                continue;
            }

            if (!anyOrdered.Contains(dish))
            {
                none.Add(dish);
            }
            else if (allOrdered.Contains(dish))
            {
                all.Add(dish);
            }
            else
            {
                some.Add(dish);
            }
        }

        return (all, some, none);
    }

    public static string FindConsonantsInOneWord(string path)
    {
        using StreamReader reader = new StreamReader(path, Encoding.UTF8);
        HashSet<char> once = new HashSet<char>();
        HashSet<char> repeated = new HashSet<char>();
        HashSet<char> wordLetters = new HashSet<char>();
        StringBuilder result = new StringBuilder();
        string? line = null;

        while ((line = reader.ReadLine()) is not null)
        {
            foreach (Match word in Regex.Matches(line.ToLowerInvariant(), "[а-яё]+"))
            {
                wordLetters.Clear();

                foreach (char letter in word.Value)
                {
                    if (RussianConsonants.Contains(letter) && wordLetters.Add(letter) &&
                        !repeated.Contains(letter) && !once.Add(letter))
                    {
                        once.Remove(letter);
                        repeated.Add(letter);
                    }
                }
            }
        }

        foreach (char letter in RussianConsonants)
        {
            if (once.Contains(letter))
            {
                result.Append(letter);
            }
        }

        return result.ToString();
    }

    public static void FillSportsFile(string path, IReadOnlyList<string> participants)
    {
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
        using StreamReader reader = new StreamReader(path, Encoding.UTF8);
        Dictionary<int, List<string>> standings = new Dictionary<int, List<string>>();
        List<string> winners = new List<string>();
        List<string>? tied = null;
        string? line = null;
        string name = string.Empty;
        int total = 0;

        if (!int.TryParse(reader.ReadLine(), NumberStyles.None, CultureInfo.InvariantCulture, out int count))
        {
            throw new InvalidDataException("Первая строка файла должна содержать количество участников.");
        }

        ValidateParticipantCount(count);

        for (int i = 0; i < count; i++)
        {
            line = reader.ReadLine() ?? throw new InvalidDataException($"Отсутствует строка участника №{i + 1}.");
            (name, total) = ParseParticipant(line);

            if (!standings.TryGetValue(total, out tied))
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
        for (int score = 40; score >= 0 && winners.Count < 3; score--)
        {
            if (standings.TryGetValue(score, out tied))
            {
                winners.AddRange(tied);
            }
        }

        return winners;
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
