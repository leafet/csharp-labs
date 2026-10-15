namespace CSharpLabs.Lab2.Task3_Cities;

public class Route
{
    private readonly City _destination;
    private int _cost;

    public Route(City destination, int cost)
    {
        _destination = destination ?? throw new ArgumentNullException(nameof(destination));
        Cost = cost;
    }

    public City Destination
    {
        get
        {
            return _destination;
        }
    }

    public int Cost
    {
        get
        {
            return _cost;
        }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Стоимость должна быть положительной.");
            }

            _cost = value;
        }
    }

    public override string ToString()
    {
        return $"{_destination.Name}:{_cost}";
    }
}
