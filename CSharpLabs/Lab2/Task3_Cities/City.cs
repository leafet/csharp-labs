using System.Text;

namespace CSharpLabs.Lab2.Task3_Cities;

public class City
{
    private string _name = string.Empty;
    private readonly List<Route> _routes;

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            string? name = value?.Trim();

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Название города не может быть пустым.", nameof(value));
            }

            _name = name;
        }
    }

    public IReadOnlyList<Route> Routes
    {
        get
        {
            return _routes.AsReadOnly();
        }
    }

    public City(string name)
    {
        Name = name;
        _routes = new List<Route>();
    }

    public void AddRoute(City destination, int cost)
    {
        if (destination is null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (ReferenceEquals(this, destination))
        {
            throw new ArgumentException("Путь должен вести в другой город.", nameof(destination));
        }

        for (int i = 0; i < _routes.Count; i++)
        {
            if (ReferenceEquals(_routes[i].Destination, destination))
            {
                throw new ArgumentException("Путь в этот город уже существует.", nameof(destination));
            }
        }

        _routes.Add(new Route(destination, cost));
    }

    public override string ToString()
    {
        StringBuilder result = new StringBuilder(_name);

        result.Append(" [");

        for (int i = 0; i < _routes.Count; i++)
        {
            if (i > 0)
            {
                result.Append(", ");
            }

            result.Append(_routes[i]);
        }

        result.Append(']');

        return result.ToString();
    }
}
