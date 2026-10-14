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
        Person petr = new Person(name: new PersonName("Пётр", "Чудов"), height: 180, father: ivan);
        Person boris = new Person(name: new PersonName("Борис"), height: 170, father: petr);

        Console.WriteLine(ivan);
        Console.WriteLine(petr);
        Console.WriteLine(boris);
    }
}
