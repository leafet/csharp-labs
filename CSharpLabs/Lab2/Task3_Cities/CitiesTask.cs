namespace CSharpLabs;

public sealed partial class Lab2
{
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
        b.AddRoute(a, 5);
        c.AddRoute(b, 3);
        c.AddRoute(d, 4);
        d.AddRoute(a, 6);
        d.AddRoute(c, 4);
        d.AddRoute(e, 2);
        f.AddRoute(b, 1);
        f.AddRoute(e, 2);

        return new[] { a, b, c, d, e, f };
    }

    private static void RunCustomCities()
    {
        City[] cities = CreateCities();
        Console.WriteLine("Добавьте свой путь к схеме.");
        City source = ReadCity(cities, "Город отправления (A-F): ");
        City destination = ReadCity(cities, "Город назначения (A-F): ");

        while (ReferenceEquals(source, destination) || HasRoute(source, destination))
        {
            Console.WriteLine("Выберите другой город: путь должен быть новым и вести в другой город.");
            destination = ReadCity(cities, "Город назначения (A-F): ");
        }

        int cost = ConsoleInput.ReadInt("Стоимость поездки: ", 1, 1_000_000);
        source.AddRoute(destination, cost);
        Console.WriteLine("После добавления: " + source);
    }

    private static City ReadCity(City[] cities, string prompt)
    {
        while (true)
        {
            string name = ConsoleInput.ReadText(prompt);

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
