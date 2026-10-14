namespace CSharpLabs;

public sealed partial class Lab2
{
    private static void RunNames()
    {
        PersonFullName cleopatra = new PersonFullName("Клеопатра");
        PersonFullName pushkin = new PersonFullName("Александр", "Пушкин", "Сергеевич");
        PersonFullName mayakovsky = new PersonFullName("Владимир", "Маяковский");

        Console.WriteLine(cleopatra);
        Console.WriteLine(pushkin);
        Console.WriteLine(mayakovsky);
    }
}
