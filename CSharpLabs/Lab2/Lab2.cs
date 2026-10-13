namespace CSharpLabs;

public sealed class Lab2
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

            int choice = ReadInt("Выберите задание: ", 0, 5);

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

    private static void RunNames()
    {
        Console.WriteLine("Введите данные трёх имён из условия.");
        PersonName cleopatra = new PersonName(ReadText("Личное имя первого человека: "));
        PersonName pushkin = new PersonName(
            ReadText("Личное имя второго человека: "),
            ReadText("Фамилия второго человека: "),
            ReadText("Отчество второго человека: "));
        PersonName mayakovsky = new PersonName(
            ReadText("Личное имя третьего человека: "),
            ReadText("Фамилия третьего человека: "));

        Console.WriteLine(cleopatra);
        Console.WriteLine(pushkin);
        Console.WriteLine(mayakovsky);
    }

    private static void RunPeople()
    {
        Console.WriteLine("Часть 2.2. Введите данные Клеопатры, Пушкина и Маяковского.");
        Person cleopatra = new Person(
            ReadName("Первый человек"), ReadInt("Рост первого человека: ", 1, 300));
        Person pushkin = new Person(
            ReadName("Второй человек"), ReadInt("Рост второго человека: ", 1, 300));
        Person mayakovsky = new Person(
            ReadName("Третий человек"), ReadInt("Рост третьего человека: ", 1, 300));
        Console.WriteLine(cleopatra);
        Console.WriteLine(pushkin);
        Console.WriteLine(mayakovsky);

        Console.WriteLine("Часть 2.3. Введите данные Ивана, Петра и Бориса.");
        Person ivan = new Person(ReadName("Иван"), ReadInt("Рост Ивана: ", 1, 300));
        Person petr = new Person(ReadName("Пётр"), ReadInt("Рост Петра: ", 1, 300), ivan);
        Person boris = new Person(ReadName("Борис"), ReadInt("Рост Бориса: ", 1, 300), petr);
        Console.WriteLine(ivan);
        Console.WriteLine(petr);
        Console.WriteLine(boris);
    }

    private static void RunCities()
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
        b.AddRoute(a, 5);
        c.AddRoute(b, 3);
        c.AddRoute(d, 4);
        d.AddRoute(a, 6);
        d.AddRoute(c, 4);
        d.AddRoute(e, 2);
        f.AddRoute(b, 1);
        f.AddRoute(e, 2);

        Console.WriteLine("Схема городов из задания:");
        City[] cities = { a, b, c, d, e, f };

        for (int i = 0; i < cities.Length; i++)
        {
            Console.WriteLine(cities[i]);
        }

        Console.WriteLine("Добавьте свой путь к схеме.");
        City source = ReadCity(cities, "Город отправления (A-F): ");
        City destination = ReadCity(cities, "Город назначения (A-F): ");

        while (ReferenceEquals(source, destination) || HasRoute(source, destination))
        {
            Console.WriteLine("Выберите другой город: путь должен быть новым и вести в другой город.");
            destination = ReadCity(cities, "Город назначения (A-F): ");
        }

        int cost = ReadInt("Стоимость поездки: ", 1, 1_000_000);
        source.AddRoute(destination, cost);
        Console.WriteLine("После добавления: " + source);
    }

    private static void RunConstructors()
    {
        Console.WriteLine("Имена из задания 4.5:");
        Console.WriteLine(new PersonName("Клеопатра"));
        Console.WriteLine(new PersonName("Александр", "Пушкин", "Сергеевич"));
        Console.WriteLine(new PersonName("Владимир", "Маяковский"));
        Console.WriteLine(new PersonName("Христофор", "Бонифатьевич"));

        Console.WriteLine("Люди из задания 4.6:");
        Person lev = new Person("Лев", 170);
        Person sergey = new Person(new PersonName("Сергей", "Пушкин"), 168, lev);
        Person alexander = new Person("Александр", 167, sergey);
        Console.WriteLine(lev);
        Console.WriteLine(sergey);
        Console.WriteLine(alexander);

        Console.WriteLine("Создайте своего человека через конструктор с именем-строкой и отцом.");
        string givenName = ReadText("Личное имя: ");
        int height = ReadInt("Рост: ", 1, 300);
        Console.WriteLine(new Person(givenName, height, alexander));
    }

    private static void RunCat()
    {
        Cat barsik = new Cat(ReadText("Имя кота (по условию — Барсик): "));
        Console.WriteLine(barsik);
        barsik.Meow();
        barsik.Meow(ReadInt("Сколько раз помяукать (по условию — 3): ", 1, 100));
    }

    private static PersonName ReadName(string label)
    {
        Console.WriteLine(label + ":");
        string givenName = ReadText("  Личное имя: ");
        string? familyName = ReadOptionalText("  Фамилия (Enter, если нет): ");
        string? patronymic = ReadOptionalText("  Отчество (Enter, если нет): ");
        return new PersonName(givenName, familyName, patronymic);
    }

    private static string ReadText(string prompt)
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

    private static string? ReadOptionalText(string prompt)
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

    private static int ReadInt(string prompt, int minimum, int maximum)
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

    private static City ReadCity(City[] cities, string prompt)
    {
        while (true)
        {
            string name = ReadText(prompt);

            for (int i = 0; i < cities.Length; i++)
            {
                if (string.Equals(cities[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return cities[i];
                }
            }

            Console.WriteLine("Введите букву от A до F.");
        }
    }

    private static bool HasRoute(City source, City destination)
    {
        for (int i = 0; i < source.Routes.Count; i++)
        {
            if (ReferenceEquals(source.Routes[i].Destination, destination))
            {
                return true;
            }
        }

        return false;
    }
}
