namespace CSharpLabs;

internal static class ConsoleInput
{
    public static string ReadText(string prompt)
    {
        while (true)
        {
            string? value = ReadOptionalText(prompt);

            if (value is not null)
            {
                return value;
            }

            Console.WriteLine("Значение не может быть пустым.");
        }
    }

    public static string? ReadOptionalText(string prompt)
    {
        Console.Write(prompt);
        string? value = Console.ReadLine();

        if (value is null)
        {
            throw new EndOfStreamException();
        }

        value = value.Trim();
        return value.Length == 0 ? null : value;
    }

    public static int ReadInt(string prompt, int minimum, int maximum)
    {
        while (true)
        {
            string? input = ReadOptionalText(prompt);

            if (int.TryParse(input, out int result) && result >= minimum && result <= maximum)
            {
                return result;
            }

            Console.WriteLine($"Введите целое число от {minimum} до {maximum}.");
        }
    }
}
