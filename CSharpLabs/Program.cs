namespace CSharpLabs;

// Единственная точка входа проекта: выбор лабораторной работы.
public static class Program
{
    /// <summary>
    /// Создаёт экземпляр класса с задачами лабораторной работы и запускает меню.
    /// </summary>
    public static void Main()
    {
        try
        {
            while (true)
            {
                Console.WriteLine("\n1 — Лабораторная работа №1");
                Console.WriteLine("2 — Лабораторная работа №2, вариант 3");
                Console.WriteLine("0 — Выход");
                Console.Write("Выберите лабораторную работу: ");

                string? input = Console.ReadLine();
                if (input is null)
                {
                    throw new EndOfStreamException();
                }

                if (!int.TryParse(input, out int choice) || choice < 0 || choice > 2)
                {
                    Console.WriteLine("Введите 0, 1 или 2.");
                    continue;
                }

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        new Lab1().Run();
                        break;
                    case 2:
                        new Lab2().Run();
                        break;
                }
            }
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("\nВвод завершён.");
        }
    }
}
