namespace CSharpLabs;

internal static class ConsoleInput
{
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
