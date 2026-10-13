namespace CSharpLabs;

public sealed partial class Lab2
{
    private static void RunCat()
    {
        Cat barsik = new Cat(ConsoleInput.ReadText("Имя кота (по условию — Барсик): "));
        Console.WriteLine(barsik);
        barsik.Meow();
        barsik.Meow(ConsoleInput.ReadInt("Сколько раз помяукать (по условию — 3): ", 1, 100));
    }
}
