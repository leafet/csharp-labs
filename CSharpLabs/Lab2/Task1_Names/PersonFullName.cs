namespace CSharpLabs.Lab2.Task1_Names;

public class PersonFullName
{
    private string _name;
    private string? _secondName;
    private string? _patronymic;

    public PersonFullName(string name, string? secondName, string? patronymic)
    {
        _name = RequireText(name, nameof(name));
        _secondName = OptionalText(secondName);
        _patronymic = OptionalText(patronymic);
    }

    public PersonFullName(string name, string? secondName) : this(name, secondName, null)
    {
    }

    public PersonFullName(string name) : this(name, null, null)
    {
    }

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = RequireText(value, nameof(Name));
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
        string result = _name;

        if (_secondName is not null)
        {
            result = _secondName + " " + result;
        }

        if (_patronymic is not null)
        {
            result += " " + _patronymic;
        }

        return result;
    }

    private static string RequireText(string? value, string parameterName)
    {
        string? result = OptionalText(value);
        return result ?? throw new ArgumentException("Текст не может быть пустым.", parameterName);
    }

    private static string? OptionalText(string? value)
    {
        string? result = value?.Trim();
        return string.IsNullOrEmpty(result) ? null : result;
    }
}
