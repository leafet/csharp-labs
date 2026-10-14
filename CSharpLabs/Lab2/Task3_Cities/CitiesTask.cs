namespace CSharpLabs;

public partial class Lab2
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
}
