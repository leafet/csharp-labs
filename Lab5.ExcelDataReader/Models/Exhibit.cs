namespace Lab5.ExcelLibrary.Models;

public sealed class Exhibit
{
    public int Id { get; }
    public string Name { get; }
    public string Era { get; }

    public Exhibit(int id, string name, string era)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(era);
        Id = id;
        Name = name.Trim();
        Era = era.Trim();
    }

    public override string ToString()
    {
        return $"{Id} | {Name} | {Era}";
    }
}
