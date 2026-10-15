namespace CSharpLabs.Lab2.Task1_Names;

public class PersonFullName
{
    private string? _name;
    private string? _secondName;
    private string? _patronymic;

    public PersonFullName(string? name, string? secondName, string? patronymic)
    {
        _name = OptionalText(name);
        _secondName = OptionalText(secondName);
        _patronymic = OptionalText(patronymic);
    }

    public PersonFullName(string? name, string? secondName) : this(name, secondName, null)
    {
    }

    public PersonFullName(string? name) : this(name, null, null)
    {
    }

    public string? Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = OptionalText(value);
        }
    }

    public string? SecondName
    {
        get
        {
            return _secondName;
        }
        set
        {
            _secondName = OptionalText(value);
        }
    }

    public string? Patronymic
    {
        get
        {
            return _patronymic;
        }
        set
        {
            _patronymic = OptionalText(value);
        }
    }

    public override string ToString()
    {
        List<string> parts = new List<string>();

        if (_secondName is not null)
        {
            parts.Add(_secondName);
        }

        if (_name is not null)
        {
            parts.Add(_name);
        }

        if (_patronymic is not null)
        {
            parts.Add(_patronymic);
        }

        return string.Join(" ", parts);
    }

    private static string? OptionalText(string? value)
    {
        string? result = value?.Trim();
        return string.IsNullOrEmpty(result) ? null : result;
    }
}
