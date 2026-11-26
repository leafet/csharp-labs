using Lab5.ExcelLibrary.Models;

namespace Lab5.ExcelLibrary.Tasks;

internal static class Task3
{
    public static List<Visitor> Execute(MuseumContext context)
    {
        return RenaissanceFilter.JuneTickets(context)
            .Join(
                context.Visitors,
                ticket => ticket.VisitorId,
                visitor => visitor.Id,
                (ticket, visitor) => visitor)
            .Distinct()
            .OrderBy(visitor => visitor.Id)
            .ToList();
    }
}
