using Lab5.ExcelLibrary.Models;

namespace Lab5.ExcelLibrary.Tasks;

internal static class Task4
{
    public static int Execute(MuseumContext context)
    {
        IEnumerable<Visitor> adults = context.Visitors.Where(visitor => visitor.Age >= 18);

        return RenaissanceFilter.JuneTickets(context)
            .Join(
                adults,
                ticket => ticket.VisitorId,
                visitor => visitor.Id,
                (ticket, visitor) => ticket)
            .Count();
    }
}
