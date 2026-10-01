namespace CSharpLabs;

// Точка входа программы. Main обязан оставаться static (требование C# к точке входа),
// все задачи лабораторной работы выполнены методами экземпляра класса Lab1.
public static class Program
{
    /// <summary>
    /// Создаёт экземпляр класса с задачами лабораторной работы и запускает меню.
    /// </summary>
    public static void Main()
    {
        Lab1 lab = new Lab1();

        try
        {
            lab.Run();
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("\nВвод завершён.");
        }
    }
}
