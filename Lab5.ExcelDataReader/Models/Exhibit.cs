namespace Lab5.ExcelLibrary.Models;

public sealed class Exhibit : Entity
{
    public string Name { get; }
    public string Era { get; }

    public Exhibit(int id, string name, string era) : base(id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(era);
        Name = name.Trim();
        Era = era.Trim();
    }

    public override string ToString()
    {
        return $"{Id} | {Name} | {Era}";
    }
}
