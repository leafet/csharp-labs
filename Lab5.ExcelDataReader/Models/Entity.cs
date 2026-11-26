namespace Lab5.ExcelLibrary.Models;

public abstract class Entity
{
    public int Id { get; }

    protected Entity(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        Id = id;
    }
}
