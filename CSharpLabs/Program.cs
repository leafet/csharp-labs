namespace CSharpLabs;

// Единственная точка входа проекта: выбор лабораторной работы.
public static class Program
{
    /// <summary>
    /// Показывает общее меню и запускает выбранную лабораторную работу.
    /// </summary>
    public static void Main()
    {
        try
        {
            while (true)
            {
                ConsoleInput.ClearScreen();
                Console.WriteLine("1 — Лабораторная работа №1");
                Console.WriteLine("2 — Лабораторная работа №2, вариант 3");
                Console.WriteLine("0 — Выход");

                int choice = ConsoleInput.ReadInt("Выберите лабораторную работу: ", 0, 2);

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
