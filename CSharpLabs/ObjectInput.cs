using CSharpLabs.Lab2.Task1_Names;
using CSharpLabs.Lab2.Task2_People;
using CSharpLabs.Lab2.Task3_Cities;
using CSharpLabs.Lab2.Task5_Cat;

namespace CSharpLabs;

internal static class ObjectInput
{
    public static PersonFullName ReadFullName()
    {
        while (true)
        {
            string? name = ConsoleInput.ReadOptionalText("Имя (Enter — пропустить): ");
            string? secondName = ConsoleInput.ReadOptionalText("Фамилия (Enter — пропустить): ");
            string? patronymic = ConsoleInput.ReadOptionalText("Отчество (Enter — пропустить): ");

            if (name is null && secondName is null && patronymic is null)
            {
                Console.WriteLine("Должна быть задана хотя бы одна часть ФИО. Повторите ввод.");
                continue;
            }

            if (patronymic is not null)
            {
                return new PersonFullName(name, secondName, patronymic);
            }

            if (secondName is not null)
            {
                return new PersonFullName(name, secondName);
            }

            return new PersonFullName(name);
        }
    }

    public static Person ReadPerson()
    {
        Console.WriteLine("1 — Строковое имя; 2 — Объект ФИО");
        int nameType = ConsoleInput.ReadInt("Способ задания имени: ", 1, 2);
        PersonFullName fullName;

        if (nameType == 1)
        {
            fullName = new PersonFullName(ConsoleInput.ReadRequiredText("Имя: "));
        }
        else
        {
            fullName = ReadFullName();
        }

        int height = ConsoleInput.ReadInt("Рост: ", 1, int.MaxValue);
        int fatherChoice = ConsoleInput.ReadInt("Указать отца? (1 — да, 0 — нет): ", 0, 1);
        Person? father = null;

        if (fatherChoice == 1)
        {
            Console.WriteLine("Данные отца:");
            PersonFullName fatherName = ReadFullName();
            int fatherHeight = ConsoleInput.ReadInt("Рост отца: ", 1, int.MaxValue);

            father = new Person(fatherName, fatherHeight);
        }

        if (nameType == 1)
        {
            return father is null
                ? new Person(fullName.Name, height)
                : new Person(fullName.Name, height, father);
        }

        return father is null
            ? new Person(fullName, height)
            : new Person(fullName, height, father);
    }

    public static City[] ReadCities()
    {
        int count = ConsoleInput.ReadInt("Количество городов (1–100): ", 1, 100);
        City[] cities = new City[count];

        for (int i = 0; i < cities.Length; i++)
        {
            string name = ConsoleInput.ReadRequiredText($"Название города №{i + 1}: ");
            cities[i] = new City(name);
        }

        for (int i = 0; i < cities.Length; i++)
        {
            Console.WriteLine($"Исходящие пути из города №{i + 1} ({cities[i].Name}):");
            int routeCount = ConsoleInput.ReadInt("Количество путей: ", 0, count - 1);
            bool[] destinations = new bool[count];

            for (int j = 0; j < routeCount; j++)
            {
                int destination = ReadDestination(cities, i, destinations);
                int cost = ConsoleInput.ReadInt("Стоимость поездки: ", 1, int.MaxValue);

                cities[i].AddRoute(cities[destination], cost);
                destinations[destination] = true;
            }
        }

        return cities;
    }

    private static int ReadDestination(City[] cities, int source, bool[] destinations)
    {
        for (int i = 0; i < cities.Length; i++)
        {
            if (i != source && !destinations[i])
            {
                Console.WriteLine($"{i + 1} — {cities[i].Name}");
            }
        }

        while (true)
        {
            int destination = ConsoleInput.ReadInt("Номер города назначения: ", 1, cities.Length) - 1;

            if (destination == source)
            {
                Console.WriteLine("Путь должен вести в другой город.");
                continue;
            }

            if (destinations[destination])
            {
                Console.WriteLine("Путь в этот город уже существует.");
                continue;
            }

            return destination;
        }
    }

    public static Cat ReadCat()
    {
        string name = ConsoleInput.ReadRequiredText("Имя кота: ");

        return new Cat(name);
    }
}
