namespace CSharpLabs;

public class Person
{
    private PersonFullName _fullName;
    private int _height;
    private Person? _father;

    public PersonFullName FullName
    {
        get
        {
            return _fullName;
        }
        set
        {
            _fullName = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public int Height
    {
        get
        {
            return _height;
        }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Рост должен быть положительным.");
            }

            _height = value;
        }
    }

    public Person? Father
    {
        get
        {
            return _father;
        }
        set
        {
            if (ReferenceEquals(this, value))
            {
                throw new ArgumentException("Человек не может быть собственным отцом.", nameof(value));
            }

            _father = value;
        }
    }

    public Person(string name, int height) : this(new PersonFullName(name), height, null)
    {
    }

    public Person(string name, int height, Person father)
        : this(new PersonFullName(name), height, father)
    {
    }

    public Person(PersonFullName fullName, int height) : this(fullName, height, null)
    {
    }

    public Person(PersonFullName fullName, int height, Person? father)
    {
        _fullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        Height = height;
        Father = father;
    }

    public override string ToString()
    {
        CompleteNameFromFather();

        return $"{_fullName}, рост: {_height}";
    }

    private void CompleteNameFromFather()
    {
        if (_father is null)
        {
            return;
        }

        if (_fullName.SecondName is null)
        {
            _fullName.SecondName = _father.FullName.SecondName;
        }

        if (_fullName.Patronymic is null)
        {
            _fullName.Patronymic = MakePatronymic(_father.FullName.Name);
        }
    }

    private static string MakePatronymic(string name)
    {
        if (string.Equals(name, "Лев", StringComparison.OrdinalIgnoreCase))
        {
            return "Львович";
        }

        if (string.Equals(name, "Пётр", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(name, "Петр", StringComparison.OrdinalIgnoreCase))
        {
            return "Петрович";
        }

        if (name.EndsWith('й'))
        {
            return name[..^1] + "евич";
        }

        return name + "ович";
    }
}
