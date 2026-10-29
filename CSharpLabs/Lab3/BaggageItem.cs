namespace CSharpLabs.Lab3;

/// <summary>Единица багажа, сериализуемая в XML.</summary>
public struct BaggageItem
{
    private string? _name;
    private decimal _mass;

    /// <summary>Создаёт единицу багажа с названием и массой в килограммах.</summary>
    public BaggageItem(string name, decimal mass)
    {
        _name = null;
        _mass = 0;
        Name = name;
        Mass = mass;
    }

    /// <summary>Название единицы багажа.</summary>
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

    /// <summary>Положительная масса в килограммах.</summary>
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
