namespace CSharpLabs;

public sealed partial class Lab2
{
    private static void RunCat()
    {
        Cat barsik = new Cat(ReadText("Имя кота (по условию — Барсик): "));
        Console.WriteLine(barsik);
        barsik.Meow();
        barsik.Meow(ReadInt("Сколько раз помяукать (по условию — 3): ", 1, 100));
    }
}
