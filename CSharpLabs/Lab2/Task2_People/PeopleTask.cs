namespace CSharpLabs;

public sealed partial class Lab2
{
    private static void RunPeople()
    {
        Console.WriteLine("Задание 2.2 — человек с именем:");

        Person cleopatra = new Person(new PersonFullName("Клеопатра"), 152);
        Person pushkin = new Person(new PersonFullName("Александр", "Пушкин", "Сергеевич"), 167);
        Person mayakovsky = new Person(new PersonFullName("Владимир", "Маяковский"), 189);

        Console.WriteLine(cleopatra);
        Console.WriteLine(pushkin);
        Console.WriteLine(mayakovsky);

        Console.WriteLine("Задание 2.3 — человек с родителем:");

        Person ivan = new Person(new PersonFullName("Иван", "Чудов"), 175);
        Person petr = new Person(new PersonFullName("Пётр", "Чудов"), 180, ivan);
        Person boris = new Person(new PersonFullName("Борис"), 170, petr);

        Console.WriteLine(ivan);
        Console.WriteLine(petr);
        Console.WriteLine(boris);
    }
}
