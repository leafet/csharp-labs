namespace CSharpLabs;

public sealed partial class Lab2
{
    private static void RunConstructors()
    {
        Console.WriteLine("Имена из задания 4.5:");

        PersonName cleopatra = new PersonName("Клеопатра");
        PersonName pushkin = new PersonName("Александр", "Пушкин", "Сергеевич");
        PersonName mayakovsky = new PersonName("Владимир", "Маяковский");
        PersonName christopher = new PersonName("Христофор", "Бонифатьевич");

        Console.WriteLine(cleopatra);
        Console.WriteLine(pushkin);
        Console.WriteLine(mayakovsky);
        Console.WriteLine(christopher);

        Console.WriteLine("Люди из задания 4.6:");

        Person lev = new Person("Лев", 170);
        Person sergey = new Person(new PersonName("Сергей", "Пушкин"), 168, lev);
        Person alexander = new Person("Александр", 167, sergey);

        Console.WriteLine(lev);
        Console.WriteLine(sergey);
        Console.WriteLine(alexander);
    }
}
