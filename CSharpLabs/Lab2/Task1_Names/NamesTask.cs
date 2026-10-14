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
}
