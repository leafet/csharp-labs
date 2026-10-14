namespace CSharpLabs;

public sealed partial class Lab2
{
    private static void RunCat()
    {
        Cat barsik = new Cat("Барсик");

        Console.WriteLine(barsik);
        barsik.Meow();
        barsik.Meow(3);
    }

    private static void RunCustomCat()
    {
        string name = ConsoleInput.ReadText("Имя кота: ");
        int count = ConsoleInput.ReadInt("Сколько раз помяукать: ", 1, 100);

        Cat cat = new Cat(name);

        Console.WriteLine(cat);
        cat.Meow();
        cat.Meow(count);
    }
}
