namespace CSharpLabs;

public sealed partial class Lab2
{
    private static void RunNames()
    {
        Console.WriteLine("Введите данные трёх имён из условия.");
        PersonName cleopatra = new PersonName(ConsoleInput.ReadText("Личное имя первого человека: "));
        PersonName pushkin = new PersonName(
            ConsoleInput.ReadText("Личное имя второго человека: "),
            ConsoleInput.ReadText("Фамилия второго человека: "),
            ConsoleInput.ReadText("Отчество второго человека: "));
        PersonName mayakovsky = new PersonName(
            ConsoleInput.ReadText("Личное имя третьего человека: "),
            ConsoleInput.ReadText("Фамилия третьего человека: "));

        Console.WriteLine(cleopatra);
        Console.WriteLine(pushkin);
        Console.WriteLine(mayakovsky);
    }
}
