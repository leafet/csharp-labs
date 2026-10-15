using System.Text;

namespace CSharpLabs.Lab2.Task3_Cities;

public class City
{
    private string _name;
    private readonly List<Route> _routes;

    public City(string name)
    {
        _name = ValidateName(name);
        _routes = new List<Route>();
    }

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = ValidateName(value);
        }
    }

    public IReadOnlyList<Route> Routes
    {
        get
        {
            return _routes.AsReadOnly();
        }
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

    private static string ValidateName(string? value)
    {
        string? name = value?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Название города не может быть пустым.", nameof(value));
        }

        return name;
    }
}
