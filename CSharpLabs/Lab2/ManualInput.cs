namespace CSharpLabs;

public partial class Lab2
{
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
