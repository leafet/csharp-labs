namespace CSharpLabs;

public sealed partial class Lab2
{
    private static void RunPeople()
    {
        Console.WriteLine("Часть 2.2. Введите данные Клеопатры, Пушкина и Маяковского.");
        Person cleopatra = new Person(
            ReadName("Первый человек"), ConsoleInput.ReadInt("Рост первого человека: ", 1, 300));
        Person pushkin = new Person(
            ReadName("Второй человек"), ConsoleInput.ReadInt("Рост второго человека: ", 1, 300));
        Person mayakovsky = new Person(
            ReadName("Третий человек"), ConsoleInput.ReadInt("Рост третьего человека: ", 1, 300));
        Console.WriteLine(cleopatra);
        Console.WriteLine(pushkin);
        Console.WriteLine(mayakovsky);

        Console.WriteLine("Часть 2.3. Введите данные Ивана, Петра и Бориса.");
        Person ivan = new Person(ReadName("Иван"), ConsoleInput.ReadInt("Рост Ивана: ", 1, 300));
        Person petr = new Person(ReadName("Пётр"), ConsoleInput.ReadInt("Рост Петра: ", 1, 300), ivan);
        Person boris = new Person(ReadName("Борис"), ConsoleInput.ReadInt("Рост Бориса: ", 1, 300), petr);
        Console.WriteLine(ivan);
        Console.WriteLine(petr);
        Console.WriteLine(boris);
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
