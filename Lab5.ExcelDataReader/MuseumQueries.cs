using Lab5.ExcelLibrary.Models;

namespace Lab5.ExcelLibrary;

internal static class MuseumQueries
{
    private static readonly DateTime JuneStart = new DateTime(2023, 6, 1);
    private static readonly DateTime JulyStart = new DateTime(2023, 7, 1);

    public static List<Exhibit> RenaissanceExhibits(MuseumContext museum)
    {
        return museum.Exhibits
            .Where(IsRenaissance)
            .OrderBy(exhibit => exhibit.Id)
            .ToList();
    }

    public static decimal RenaissanceRevenue(MuseumContext museum)
    {
        return JuneRenaissanceTickets(museum).Sum(ticket => ticket.Price);
    }

    public static List<Visitor> RenaissanceVisitors(MuseumContext museum)
    {
        return JuneRenaissanceTickets(museum)
            .Join(museum.Visitors,
                ticket => ticket.VisitorId,
                visitor => visitor.Id,
                (ticket, visitor) => visitor)
            .DistinctBy(visitor => visitor.Id)
            .OrderBy(visitor => visitor.Id)
            .ToList();
    }

    public static int AdultTicketCount(MuseumContext museum)
    {
        IEnumerable<Visitor> adults = museum.Visitors.Where(visitor => visitor.Age >= 18);

        return JuneRenaissanceTickets(museum)
            .Join(adults,
                ticket => ticket.VisitorId,
                visitor => visitor.Id,
                (ticket, visitor) => ticket)
            .Count();
    }

    private static IEnumerable<Ticket> JuneRenaissanceTickets(MuseumContext museum)
    {
        IEnumerable<Ticket> juneTickets = museum.Tickets
            .Where(ticket => ticket.Date >= JuneStart && ticket.Date < JulyStart);
        IEnumerable<Exhibit> exhibits = museum.Exhibits.Where(IsRenaissance);

        return juneTickets.Join(exhibits,
            ticket => ticket.ExhibitId,
            exhibit => exhibit.Id,
            (ticket, exhibit) => ticket);
    }

    private static bool IsRenaissance(Exhibit exhibit)
    {
        return exhibit.Era.Equals("Ренессанс", StringComparison.OrdinalIgnoreCase);
    }
}
