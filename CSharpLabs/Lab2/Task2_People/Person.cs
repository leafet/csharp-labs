namespace CSharpLabs;

public sealed class Person
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
            Person? ancestor = value;

            while (ancestor is not null)
            {
                if (ReferenceEquals(this, ancestor))
                {
                    throw new ArgumentException("Родственные связи не могут образовывать цикл.", nameof(value));
                }

                ancestor = ancestor.Father;
            }

            _father = value;
        }
    }

    public Person(string givenName, int height) : this(new PersonFullName(givenName), height, null)
    {
    }

    public Person(string givenName, int height, Person father)
        : this(new PersonFullName(givenName), height, father)
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

        _father.CompleteNameFromFather();

        if (_fullName.FamilyName is null)
        {
            _fullName.FamilyName = _father.FullName.FamilyName;
        }

        if (_fullName.Patronymic is null)
        {
            _fullName.Patronymic = MakePatronymic(_father.FullName.GivenName);
        }
    }

    private static string MakePatronymic(string givenName)
    {
        if (string.Equals(givenName, "Лев", StringComparison.OrdinalIgnoreCase))
        {
            return "Львович";
        }

        if (string.Equals(givenName, "Пётр", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(givenName, "Петр", StringComparison.OrdinalIgnoreCase))
        {
            return "Петрович";
        }

        if (givenName.EndsWith('й'))
        {
            return givenName[..^1] + "евич";
        }

        return givenName + "ович";
    }
}
