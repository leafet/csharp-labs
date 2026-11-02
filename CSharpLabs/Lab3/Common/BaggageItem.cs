namespace CSharpLabs.Lab3.Common;

public struct BaggageItem
{
    private string? _name;
    private decimal _mass;

    public BaggageItem(string name, decimal mass)
    {
        _name = null;
        _mass = 0;
        Name = name;
        Mass = mass;
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

    public decimal Mass
    {
        get
        {
            return _mass;
        }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Масса должна быть положительной.");
            }

            _mass = value;
        }
    }
}
