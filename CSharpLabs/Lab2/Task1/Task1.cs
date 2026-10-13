namespace CSharpLabs;

public sealed partial class Lab2
{
    private static void RunNames()
    {
        Console.WriteLine("Введите данные трёх имён из условия.");
        PersonName cleopatra = new PersonName(ReadText("Личное имя первого человека: "));
        PersonName pushkin = new PersonName(
            ReadText("Личное имя второго человека: "),
            ReadText("Фамилия второго человека: "),
            ReadText("Отчество второго человека: "));
        PersonName mayakovsky = new PersonName(
            ReadText("Личное имя третьего человека: "),
            ReadText("Фамилия третьего человека: "));

        Console.WriteLine(cleopatra);
        Console.WriteLine(pushkin);
        Console.WriteLine(mayakovsky);
    }
}
