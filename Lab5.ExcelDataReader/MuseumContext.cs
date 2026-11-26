using Lab5.ExcelLibrary.Models;

namespace Lab5.ExcelLibrary;

public sealed partial class MuseumContext : FileDbContext
{
    public IReadOnlyList<Exhibit> Exhibits => Set<Exhibit>();
    public IReadOnlyList<Visitor> Visitors => Set<Visitor>();
    public IReadOnlyList<Ticket> Tickets => Set<Ticket>();

    private MuseumContext(string path, List<Exhibit> exhibits, List<Visitor> visitors, List<Ticket> tickets)
        : base(path)
    {
        RegisterSet(exhibits, validateRemove: EnsureExhibitHasNoTickets);
        RegisterSet(visitors, validateRemove: EnsureVisitorHasNoTickets);
        RegisterSet(tickets, validateWrite: ValidateTicketReferences);
    }

    private void ValidateTicketReferences(Ticket ticket)
    {
        if (Find<Exhibit>(ticket.ExhibitId) is null || Find<Visitor>(ticket.VisitorId) is null)
        {
            throw new ArgumentException("Сначала добавьте посетителя и экспонат, указанные в билете.");
        }
    }

    private void EnsureExhibitHasNoTickets(Exhibit exhibit)
    {
        if (Tickets.Any(ticket => ticket.ExhibitId == exhibit.Id))
        {
            throw new InvalidOperationException("На экспонат есть билеты. Сначала удалите связанные билеты.");
        }
    }

    private void EnsureVisitorHasNoTickets(Visitor visitor)
    {
        if (Tickets.Any(ticket => ticket.VisitorId == visitor.Id))
        {
            throw new InvalidOperationException("У посетителя есть билеты. Сначала удалите связанные билеты.");
        }
    }
}
