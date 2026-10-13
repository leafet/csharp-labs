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
            Console.WriteLine("0 — Назад");

            int choice = ConsoleInput.ReadInt("Выберите задание: ", 0, 5);

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
            }
        }
    }
}
