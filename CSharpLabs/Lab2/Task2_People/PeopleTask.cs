namespace CSharpLabs;

public sealed partial class Lab2
{
    private static void RunPeople()
    {
        Console.WriteLine("Задание 2.2 — человек с именем:");

        Person cleopatra = new Person(new PersonName("Клеопатра"), 152);
        Person pushkin = new Person(new PersonName("Александр", "Пушкин", "Сергеевич"), 167);
        Person mayakovsky = new Person(new PersonName("Владимир", "Маяковский"), 189);

        Console.WriteLine(cleopatra);
        Console.WriteLine(pushkin);
        Console.WriteLine(mayakovsky);

        Console.WriteLine("Задание 2.3 — человек с родителем:");

        Person ivan = new Person(new PersonName("Иван", "Чудов"), 175);
        Person petr = new Person(new PersonName("Пётр", "Чудов"), 180, ivan);
        Person boris = new Person(new PersonName("Борис"), 170, petr);

        Console.WriteLine(ivan);
        Console.WriteLine(petr);
        Console.WriteLine(boris);
    }

    private static void RunCustomPeople()
    {
        Console.WriteLine("Часть 2.2. Введите данные трёх людей.");

        PersonName firstName = ReadName("Первый человек");
        int firstHeight = ConsoleInput.ReadInt("Рост первого человека: ", 1, 300);

        PersonName secondName = ReadName("Второй человек");
        int secondHeight = ConsoleInput.ReadInt("Рост второго человека: ", 1, 300);

        PersonName thirdName = ReadName("Третий человек");
        int thirdHeight = ConsoleInput.ReadInt("Рост третьего человека: ", 1, 300);

        Person firstPerson = new Person(firstName, firstHeight);
        Person secondPerson = new Person(secondName, secondHeight);
        Person thirdPerson = new Person(thirdName, thirdHeight);

        Console.WriteLine(firstPerson);
        Console.WriteLine(secondPerson);
        Console.WriteLine(thirdPerson);

        Console.WriteLine("Часть 2.3. Введите данные трёх поколений семьи.");

        PersonName grandfatherName = ReadName("Дед");
        int grandfatherHeight = ConsoleInput.ReadInt("Рост деда: ", 1, 300);

        PersonName fatherName = ReadName("Отец");
        int fatherHeight = ConsoleInput.ReadInt("Рост отца: ", 1, 300);

        PersonName childName = ReadName("Ребёнок");
        int childHeight = ConsoleInput.ReadInt("Рост ребёнка: ", 1, 300);

        Person grandfather = new Person(grandfatherName, grandfatherHeight);
        Person father = new Person(fatherName, fatherHeight, grandfather);
        Person child = new Person(childName, childHeight, father);

        Console.WriteLine(grandfather);
        Console.WriteLine(father);
        Console.WriteLine(child);
    }

    private static PersonName ReadName(string label)
    {
        Console.WriteLine(label + ":");

        string givenName = ConsoleInput.ReadText("  Личное имя: ");
        string? familyName = ConsoleInput.ReadOptionalText("  Фамилия (Enter, если нет): ");
        string? patronymic = ConsoleInput.ReadOptionalText("  Отчество (Enter, если нет): ");

        return new PersonName(givenName, familyName, patronymic);
    }
}
