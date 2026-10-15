using CSharpLabs.Lab2.Task1_Names;
using CSharpLabs.Lab2.Task2_People;
using CSharpLabs.Lab2.Task3_Cities;
using CSharpLabs.Lab2.Task5_Cat;

namespace CSharpLabs.Lab2;

public class Lab2
{
    public void Run()
    {
        while (true)
        {
            ConsoleInput.ClearScreen();
            Console.WriteLine("Лабораторная работа №2 — вариант 3");
            Console.WriteLine("1 — Имена (задание 1.3)");
            Console.WriteLine("2 — Человек с именем и отцом (задания 2.2 и 2.3)");
            Console.WriteLine("3 — Города (задание 3.3)");
            Console.WriteLine("4 — Конструкторы имени и человека (задания 4.5 и 4.6)");
            Console.WriteLine("5 — Кот мяукает (задание 5.2)");
            Console.WriteLine("6 — Ручной ввод объектов");
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
                    RunManualInput();
                    continue;
            }

            ConsoleInput.WaitForContinue();
        }
    }

    private static void RunNames()
    {
        PersonFullName cleopatra = new PersonFullName("Клеопатра");
        PersonFullName pushkin = new PersonFullName("Александр", "Пушкин", "Сергеевич");
        PersonFullName mayakovsky = new PersonFullName("Владимир", "Маяковский");

        Console.WriteLine(cleopatra);
        Console.WriteLine(pushkin);
        Console.WriteLine(mayakovsky);
    }

    private static void RunPeople()
    {
        Console.WriteLine("Задание 2.2 — человек с именем:");

        Person cleopatra = new Person(new PersonFullName("Клеопатра"), 152);
        Person pushkin = new Person(new PersonFullName("Александр", "Пушкин", "Сергеевич"), 167);
        Person mayakovsky = new Person(new PersonFullName("Владимир", "Маяковский"), 189);

        Console.WriteLine(cleopatra);
        Console.WriteLine(pushkin);
        Console.WriteLine(mayakovsky);

        Console.WriteLine("Задание 2.3 — человек с родителем:");

        Person ivan = new Person(new PersonFullName("Иван", "Чудов"), 175);
        Person petr = new Person(new PersonFullName("Пётр", "Чудов"), 180, ivan);
        Person boris = new Person(new PersonFullName("Борис"), 170, petr);

        Console.WriteLine(ivan);
        Console.WriteLine(petr);
        Console.WriteLine(boris);
    }

    private static void RunCities()
    {
        City[] cities = CreateCities();

        Console.WriteLine("Схема городов из задания:");

        for (int i = 0; i < cities.Length; i++)
        {
            Console.WriteLine(cities[i]);
        }
    }

    private static City[] CreateCities()
    {
        City a = new City("A");
        City b = new City("B");
        City c = new City("C");
        City d = new City("D");
        City e = new City("E");
        City f = new City("F");

        // Ориентированные пути с рисунка 2; обратный путь добавляется отдельно.
        a.AddRoute(b, 5);
        a.AddRoute(f, 1);
        a.AddRoute(d, 6);

        b.AddRoute(a, 5);
        b.AddRoute(c, 3);

        c.AddRoute(b, 3);
        c.AddRoute(d, 4);

        d.AddRoute(a, 6);
        d.AddRoute(c, 4);
        d.AddRoute(e, 2);

        e.AddRoute(f, 2);

        f.AddRoute(b, 1);
        f.AddRoute(e, 2);

        return new[] { a, b, c, d, e, f };
    }

    private static void RunConstructors()
    {
        Console.WriteLine("Имена из задания 4.5:");

        PersonFullName cleopatra = new PersonFullName("Клеопатра");
        PersonFullName pushkin = new PersonFullName("Александр", "Пушкин", "Сергеевич");
        PersonFullName mayakovsky = new PersonFullName("Владимир", "Маяковский");
        PersonFullName christopher = new PersonFullName("Христофор", "Бонифатьевич");

        Console.WriteLine(cleopatra);
        Console.WriteLine(pushkin);
        Console.WriteLine(mayakovsky);
        Console.WriteLine(christopher);

        Console.WriteLine("Люди из задания 4.6:");

        Person lev = new Person("Лев", 170);
        Person sergey = new Person(new PersonFullName("Сергей", "Пушкин"), 168, lev);
        Person alexander = new Person("Александр", 167, sergey);

        Console.WriteLine(lev);
        Console.WriteLine(sergey);
        Console.WriteLine(alexander);
    }

    private static void RunCat()
    {
        Cat barsik = new Cat("Барсик");

        Console.WriteLine(barsik);
        barsik.Meow();
        barsik.Meow(3);
    }

    private static void RunManualInput()
    {
        while (true)
        {
            ConsoleInput.ClearScreen();
            Console.WriteLine("Ручной ввод объектов ЛР2");
            Console.WriteLine("1 — ФИО (задания 1.3 и 4.5)");
            Console.WriteLine("2 — Человек (задания 2.2, 2.3 и 4.6)");
            Console.WriteLine("3 — Города и пути (задание 3.3)");
            Console.WriteLine("4 — Кот (задание 5.2)");
            Console.WriteLine("0 — Назад");

            int choice = ConsoleInput.ReadInt("Выберите тип объекта: ", 0, 4);

            switch (choice)
            {
                case 0:
                    return;
                case 1:
                    PersonFullName fullName = ObjectInput.ReadFullName();

                    Console.WriteLine(fullName);
                    break;
                case 2:
                    Person person = ObjectInput.ReadPerson();

                    if (person.Father is not null)
                    {
                        Console.WriteLine(person.Father);
                    }

                    Console.WriteLine(person);
                    break;
                case 3:
                    City[] cities = ObjectInput.ReadCities();

                    for (int i = 0; i < cities.Length; i++)
                    {
                        Console.WriteLine(cities[i]);
                    }

                    break;
                case 4:
                    Cat cat = ObjectInput.ReadCat();
                    int count = ConsoleInput.ReadInt("Количество мяуканий (1–100): ", 1, 100);

                    Console.WriteLine(cat);
                    cat.Meow(count);
                    break;
            }

            ConsoleInput.WaitForContinue();
        }
    }
}
