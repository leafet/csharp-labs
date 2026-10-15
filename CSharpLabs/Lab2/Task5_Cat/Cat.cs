using System.Text;

namespace CSharpLabs.Lab2.Task5_Cat;

public class Cat
{
    private string _name;

    public Cat(string name)
    {
        _name = ValidateName(name);
    }

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = ValidateName(value);
        }
    }

    public void Meow()
    {
        Meow(1);
    }

    public void Meow(int count)
    {
        if (count < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Количество мяуканий должно быть положительным.");
        }

        StringBuilder sound = new StringBuilder(_name + ": ");

        for (int i = 0; i < count; i++)
        {
            if (i > 0)
            {
                sound.Append('-');
            }

            sound.Append("мяу");
        }

        sound.Append('!');

        Console.WriteLine(sound);
    }

    public override string ToString()
    {
        return "кот: " + _name;
    }

    private static string ValidateName(string? value)
    {
        string? name = value?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Имя кота не может быть пустым.", nameof(value));
        }

        return name;
    }
}
