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
        Cat cat = new Cat(ConsoleInput.ReadText("Имя кота: "));
        Console.WriteLine(cat);
        cat.Meow();
        cat.Meow(ConsoleInput.ReadInt("Сколько раз помяукать: ", 1, 100));
    }
}
