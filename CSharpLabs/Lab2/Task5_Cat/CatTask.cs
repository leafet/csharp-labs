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
}
