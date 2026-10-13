using System.Text;

namespace CSharpLabs;

public sealed class Cat
{
    private string _name = string.Empty;

    public string Name
    {
        get { return _name; }
        set
        {
            string? name = value?.Trim();
            _name = string.IsNullOrEmpty(name)
                ? throw new ArgumentException("Имя кота не может быть пустым.", nameof(value))
                : name;
        }
    }

    public Cat(string name)
    {
        Name = name;
    }

    public void Meow()
    {
        Meow(1);
    }

    public void Meow(int count)
    {
        if (count < 1 || count > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Количество должно быть от 1 до 100.");
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

        Console.WriteLine(sound.Append('!'));
    }

    public override string ToString()
    {
        return "кот: " + _name;
    }
}
