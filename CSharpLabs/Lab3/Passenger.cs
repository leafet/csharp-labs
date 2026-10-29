namespace CSharpLabs.Lab3;

/// <summary>Пассажир и массив единиц его багажа для XML-сериализации.</summary>
public struct Passenger
{
    private string? _name;
    private BaggageItem[]? _items;

    /// <summary>Создаёт запись пассажира.</summary>
    public Passenger(string name, BaggageItem[] items)
    {
        _name = null;
        _items = null;
        Name = name;
        Items = items;
    }

    /// <summary>Имя пассажира.</summary>
    public string Name
    {
        get
        {
            return _name ?? string.Empty;
        }
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _name = value.Trim();
        }
    }

    /// <summary>Массив единиц багажа; пустой массив означает отсутствие багажа.</summary>
    public BaggageItem[] Items
    {
        get
        {
            return _items ?? Array.Empty<BaggageItem>();
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _items = value;
        }
    }
}
