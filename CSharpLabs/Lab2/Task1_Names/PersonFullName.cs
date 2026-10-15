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
        ValidateParts(_name, _secondName, _patronymic);
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
            string? text = OptionalText(value);
            ValidateParts(text, _secondName, _patronymic);

            _name = text;
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
            string? text = OptionalText(value);
            ValidateParts(_name, text, _patronymic);

            _secondName = text;
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
            string? text = OptionalText(value);
            ValidateParts(_name, _secondName, text);

            _patronymic = text;
        }
    }

    public override string ToString()
    {
        string result = _secondName ?? string.Empty;

        if (_name is not null)
        {
            result += result.Length > 0 ? " " + _name : _name;
        }

        if (_patronymic is not null)
        {
            result += result.Length > 0 ? " " + _patronymic : _patronymic;
        }

        return result;
    }

    private static void ValidateParts(string? name, string? secondName, string? patronymic)
    {
        if (name is null && secondName is null && patronymic is null)
        {
            throw new ArgumentException("Должна быть задана хотя бы одна часть ФИО.");
        }
    }

    private static string? OptionalText(string? value)
    {
        string? result = value?.Trim();
        return string.IsNullOrEmpty(result) ? null : result;
    }
}
