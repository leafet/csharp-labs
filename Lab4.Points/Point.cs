using System.Globalization;

namespace CSharpLabs.Lab4.Points;

/// <summary>Точка на плоскости с вещественными координатами.</summary>
public sealed class Point
{
    private double _x;
    private double _y;

    public Point() : this(0, 0)
    {
    }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X
    {
        get { return _x; }
        set
        {
            ValidateCoordinate(value, nameof(X));
            _x = value;
        }
    }

    public double Y
    {
        get { return _y; }
        set
        {
            ValidateCoordinate(value, nameof(Y));
            _y = value;
        }
    }

    public double DistanceTo(Point other)
    {
        ArgumentNullException.ThrowIfNull(other);
        double deltaX = X - other.X;
        double deltaY = Y - other.Y;
        return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

    public override string ToString()
    {
        return string.Create(CultureInfo.InvariantCulture, $"({X}; {Y})");
    }

    public static Point operator ++(Point point)
    {
        return point + 1;
    }

    public static Point operator --(Point point)
    {
        return point + (-1);
    }

    public static explicit operator int(Point point)
    {
        ArgumentNullException.ThrowIfNull(point);
        return checked((int)point.X);
    }

    public static implicit operator double(Point point)
    {
        ArgumentNullException.ThrowIfNull(point);
        return point.Y;
    }

    public static double operator +(Point first, Point second)
    {
        ArgumentNullException.ThrowIfNull(first);
        return first.DistanceTo(second);
    }

    public static Point operator +(Point point, int offset)
    {
        ArgumentNullException.ThrowIfNull(point);
        return new Point(point.X + offset, point.Y);
    }

    public static Point operator +(int offset, Point point)
    {
        return point + offset;
    }

    private static void ValidateCoordinate(double value, string name)
    {
        if (!double.IsFinite(value))
        {
            throw new ArgumentOutOfRangeException(name, "Координата должна быть конечным числом.");
        }
    }
}
