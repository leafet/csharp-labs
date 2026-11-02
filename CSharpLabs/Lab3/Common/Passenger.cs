namespace CSharpLabs.Lab3.Common;

public struct Passenger
{
    private string? _name;
    private BaggageItem[]? _items;

    public Passenger(string name, BaggageItem[] items)
    {
        _name = null;
        _items = null;
        Name = name;
        Items = items;
    }

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
