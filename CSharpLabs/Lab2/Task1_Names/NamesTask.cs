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
        PersonName firstName = new PersonName(ConsoleInput.ReadText("Личное имя первого человека: "));
        PersonName secondName = new PersonName(
            ConsoleInput.ReadText("Личное имя второго человека: "),
            ConsoleInput.ReadText("Фамилия второго человека: "),
            ConsoleInput.ReadText("Отчество второго человека: "));
        PersonName thirdName = new PersonName(
            ConsoleInput.ReadText("Личное имя третьего человека: "),
            ConsoleInput.ReadText("Фамилия третьего человека: "));

        Console.WriteLine(firstName);
        Console.WriteLine(secondName);
        Console.WriteLine(thirdName);
    }
}
