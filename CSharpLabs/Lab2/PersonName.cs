namespace CSharpLabs;

public sealed class PersonName
{
    private string _givenName;
    private string? _familyName;
    private string? _patronymic;

    public string GivenName
    {
        get { return _givenName; }
        set { _givenName = RequireText(value, nameof(GivenName)); }
    }

    public string? FamilyName
    {
        get { return _familyName; }
        set { _familyName = OptionalText(value); }
    }

    public string? Patronymic
    {
        get { return _patronymic; }
        set { _patronymic = OptionalText(value); }
    }

    public PersonName(string givenName) : this(givenName, null, null)
    {
    }

    public PersonName(string givenName, string? familyName) : this(givenName, familyName, null)
    {
    }

    public PersonName(string givenName, string? familyName, string? patronymic)
    {
        _givenName = RequireText(givenName, nameof(givenName));
        _familyName = OptionalText(familyName);
        _patronymic = OptionalText(patronymic);
    }

    public override string ToString()
    {
        string result = _givenName;

        if (_familyName is not null)
        {
            result = _familyName + " " + result;
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
