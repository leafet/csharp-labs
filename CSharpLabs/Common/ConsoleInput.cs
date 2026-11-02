namespace CSharpLabs.Common;

internal static class ConsoleInput
{
    public static void ClearScreen()
    {
        if (Console.IsOutputRedirected)
        {
            Console.WriteLine();
            return;
        }

        try
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
        }
        catch (IOException)
        {
            // Некоторые терминалы не поддерживают управление положением курсора.
        }
        catch (ArgumentOutOfRangeException)
        {
            // Размер терминала мог измениться во время очистки.
        }
    }

    public static void WaitForContinue()
    {
        if (Console.IsInputRedirected || Console.IsOutputRedirected)
        {
            return;
        }

        Console.Write("\nНажмите Enter, чтобы вернуться в меню...");

        if (Console.ReadLine() is null)
        {
            throw new EndOfStreamException();
        }
    }

    public static int ReadInt(string prompt, int minimum, int maximum)
    {
        string? input;
        int result;

        while (true)
        {
            Console.Write(prompt);
            input = Console.ReadLine();

            if (input is null)
            {
                throw new EndOfStreamException();
            }

            if (int.TryParse(input, out result) && result >= minimum && result <= maximum)
            {
                return result;
            }

            Console.WriteLine($"Введите целое число от {minimum} до {maximum}.");
        }
    }

    public static string ReadRequiredText(string prompt)
    {
        string? text;

        while (true)
        {
            text = ReadOptionalText(prompt);

            if (text is not null)
            {
                return text;
            }

            Console.WriteLine("Значение не может быть пустым.");
        }
    }

    public static string? ReadOptionalText(string prompt)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        if (input is null)
        {
            throw new EndOfStreamException();
        }

        string text = input.Trim();

        if (text.Length == 0)
        {
            return null;
        }
        else
        {
            return text;
        }
    }
}
