namespace CSharpLabs;

public sealed partial class Lab2
{
    private static void RunNames()
    {
        PersonName cleopatra = new PersonName("Клеопатра");
        PersonName pushkin = new PersonName("Александр", "Пушкин", "Сергеевич");
        PersonName mayakovsky = new PersonName("Владимир", "Маяковский");

        Console.WriteLine(cleopatra);
        Console.WriteLine(pushkin);
        Console.WriteLine(mayakovsky);
    }

    private static void RunCustomNames()
    {
        Console.WriteLine("Введите три собственных имени.");

        string firstGivenName = ConsoleInput.ReadText("Личное имя первого человека: ");

        string secondGivenName = ConsoleInput.ReadText("Личное имя второго человека: ");
        string secondFamilyName = ConsoleInput.ReadText("Фамилия второго человека: ");
        string secondPatronymic = ConsoleInput.ReadText("Отчество второго человека: ");

        string thirdGivenName = ConsoleInput.ReadText("Личное имя третьего человека: ");
        string thirdFamilyName = ConsoleInput.ReadText("Фамилия третьего человека: ");

        PersonName firstName = new PersonName(firstGivenName);
        PersonName secondName = new PersonName(
            secondGivenName, secondFamilyName, secondPatronymic);
        PersonName thirdName = new PersonName(thirdGivenName, thirdFamilyName);

        Console.WriteLine(firstName);
        Console.WriteLine(secondName);
        Console.WriteLine(thirdName);
    }
}
