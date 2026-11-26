namespace Lab5.ExcelLibrary.Models;

public sealed class Ticket : Entity
{
    public int ExhibitId { get; }
    public int VisitorId { get; }
    public DateTime Date { get; }
    public decimal Price { get; }

    public Ticket(int id, int exhibitId, int visitorId, DateTime date, decimal price) : base(id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(exhibitId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(visitorId);
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        if (date.Date < new DateTime(1900, 1, 1))
        {
            throw new ArgumentOutOfRangeException(nameof(date), "Дата посещения должна быть не ранее 1900 года.");
        }
        if (decimal.Round(price, 2) != price || price > 1_000_000_000m)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Стоимость должна быть до 1 000 000 000 рублей с точностью до копеек.");
        }
        ExhibitId = exhibitId;
        VisitorId = visitorId;
        Date = date.Date;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Id} | {ExhibitId} | {VisitorId} | {Date:dd.MM.yyyy} | {Price:F2}";
    }
}
