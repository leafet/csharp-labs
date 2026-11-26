namespace Lab5.ExcelLibrary.Models;

public sealed class Visitor : Entity
{
    public string Name { get; }
    public int Age { get; }
    public string City { get; }

    public Visitor(int id, string name, int age, string city) : base(id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(city);
        if (age < 0 || age > 150)
        {
            throw new ArgumentOutOfRangeException(nameof(age), "Возраст должен быть от 0 до 150.");
        }
        Name = name.Trim();
        Age = age;
        City = city.Trim();
    }

    public override string ToString()
    {
        return $"{Id} | {Name} | {Age} | {City}";
    }
}
