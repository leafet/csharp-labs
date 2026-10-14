namespace CSharpLabs;

public sealed partial class Lab2
{
    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\nЛабораторная работа №2 — вариант 3");
            Console.WriteLine("1 — Имена (задание 1.3)");
            Console.WriteLine("2 — Человек с именем и отцом (задания 2.2 и 2.3)");
            Console.WriteLine("3 — Города (задание 3.3)");
            Console.WriteLine("4 — Конструкторы имени и человека (задания 4.5 и 4.6)");
            Console.WriteLine("5 — Кот мяукает (задание 5.2)");
            Console.WriteLine("6 — Ввести собственные данные для задания");
            Console.WriteLine("0 — Назад");

            int choice = ConsoleInput.ReadInt("Выберите задание: ", 0, 6);

            switch (choice)
            {
                case 0:
                    return;
                case 1:
                    RunNames();
                    break;
                case 2:
                    RunPeople();
                    break;
                case 3:
                    RunCities();
                    break;
                case 4:
                    RunConstructors();
                    break;
                case 5:
                    RunCat();
                    break;
                case 6:
                    RunCustomExamples();
                    break;
            }
        }
    }

    private static void RunCustomExamples()
    {
        while (true)
        {
            Console.WriteLine("\nВвод собственных данных");
            Console.WriteLine("1 — Имена");
            Console.WriteLine("2 — Люди и родственные связи");
            Console.WriteLine("3 — Путь между городами");
            Console.WriteLine("4 — Человек с отцом");
            Console.WriteLine("5 — Кот мяукает");
            Console.WriteLine("0 — Назад");

            int choice = ConsoleInput.ReadInt("Выберите задание: ", 0, 5);

            switch (choice)
            {
                case 0:
                    return;
                case 1:
                    RunCustomNames();
                    break;
                case 2:
                    RunCustomPeople();
                    break;
                case 3:
                    RunCustomCities();
                    break;
                case 4:
                    RunCustomConstructors();
                    break;
                case 5:
                    RunCustomCat();
                    break;
            }
        }
    }
}
