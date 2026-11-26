using Lab5.ExcelLibrary.Models;

namespace Lab5.ExcelLibrary.Tasks;

internal static class RenaissanceFilter
{
    private static readonly DateTime JuneStart = new DateTime(2023, 6, 1);
    private static readonly DateTime JulyStart = new DateTime(2023, 7, 1);

    public static IEnumerable<Ticket> JuneTickets(MuseumContext context)
    {
        IEnumerable<Ticket> juneTickets = context.Tickets.Where(ticket => ticket.Date >= JuneStart && ticket.Date < JulyStart);
        IEnumerable<Exhibit> renaissanceExhibits = context.Exhibits.Where(IsRenaissanceExhibit);

        return juneTickets.Join(
            renaissanceExhibits,
            ticket => ticket.ExhibitId,
            exhibit => exhibit.Id,
            (ticket, exhibit) => ticket);
    }

    public static bool IsRenaissanceExhibit(Exhibit exhibit)
    {
        return exhibit.Era.Equals("Ренессанс", StringComparison.OrdinalIgnoreCase);
    }
}
