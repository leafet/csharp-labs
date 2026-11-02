using CSharpLabs.Lab3.Common;

namespace CSharpLabs.Lab3.Tasks;

internal static class Task5
{
    public static void RunExample()
    {
        Passenger[] passengers =
        {
            new Passenger("Анна", new[] { new BaggageItem("Сумка", 4) }),
            new Passenger("Борис", new[] { new BaggageItem("Чемодан", 12), new BaggageItem("Коробка", 2) }),
            new Passenger("Вера", new[] { new BaggageItem("Рюкзак", 5) })
        };
        Run(passengers, 5);
    }

    public static void Run(Passenger[] passengers, decimal m)
    {
        string path = Lab3Files.GetDataPath("passengers.bin");
        FileTasks.FillPassengers(path, passengers);
        Console.WriteLine($"Файл: {path}; XML-сериализация массива структур, затем чтение файла.");

        foreach (Passenger passenger in FileTasks.ReadPassengers(path))
        {
            Console.Write($"{passenger.Name}: {passenger.Items.Length} ед.");

            foreach (BaggageItem item in passenger.Items)
            {
                Console.Write($"; {item.Name} — {item.Mass} кг");
            }

            Console.WriteLine();
        }

        bool found = FileTasks.HasLightSingleBaggage(path, m);
        Console.WriteLine($"Одна единица багажа массой менее {m} кг: {(found ? "Да" : "Нет")}");
    }
}
