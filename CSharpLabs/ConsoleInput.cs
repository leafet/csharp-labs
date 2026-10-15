namespace CSharpLabs;

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
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (input is null)
            {
                throw new EndOfStreamException();
            }

            if (int.TryParse(input, out int result) && result >= minimum && result <= maximum)
            {
                return result;
            }

            Console.WriteLine($"Введите целое число от {minimum} до {maximum}.");
        }
    }
}
