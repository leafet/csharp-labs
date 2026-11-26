using Lab5.ExcelLibrary.Models;

namespace Lab5.ExcelLibrary;

public sealed partial class MuseumContext
{
    private readonly List<Exhibit> _exhibits;
    private readonly List<Visitor> _visitors;
    private readonly List<Ticket> _tickets;

    public IReadOnlyList<Exhibit> Exhibits => _exhibits;
    public IReadOnlyList<Visitor> Visitors => _visitors;
    public IReadOnlyList<Ticket> Tickets => _tickets;
    public string FilePath { get; private set; }

    private MuseumContext(string path, List<Exhibit> exhibits, List<Visitor> visitors, List<Ticket> tickets)
    {
        FilePath = Path.GetFullPath(path);
        _exhibits = exhibits;
        _visitors = visitors;
        _tickets = tickets;
    }

    public void AddExhibit(Exhibit exhibit)
    {
        ArgumentNullException.ThrowIfNull(exhibit);
        if (_exhibits.Any(item => item.Id == exhibit.Id))
        {
            throw new ArgumentException("Запись с таким ID уже существует.");
        }
        _exhibits.Add(exhibit);
    }

    public void AddVisitor(Visitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        if (_visitors.Any(item => item.Id == visitor.Id))
        {
            throw new ArgumentException("Запись с таким ID уже существует.");
        }
        _visitors.Add(visitor);
    }

    public void AddTicket(Ticket ticket)
    {
        ArgumentNullException.ThrowIfNull(ticket);
        if (_tickets.Any(item => item.Id == ticket.Id))
        {
            throw new ArgumentException("Запись с таким ID уже существует.");
        }
        ValidateTicketReferences(ticket);
        _tickets.Add(ticket);
    }

    public bool RemoveExhibit(int id)
    {
        int index = _exhibits.FindIndex(item => item.Id == id);
        if (index < 0)
        {
            return false;
        }
        if (_tickets.Any(ticket => ticket.ExhibitId == id))
        {
            throw new InvalidOperationException("На экспонат есть билеты. Сначала удалите связанные билеты.");
        }
        _exhibits.RemoveAt(index);
        return true;
    }

    public bool RemoveVisitor(int id)
    {
        int index = _visitors.FindIndex(item => item.Id == id);
        if (index < 0)
        {
            return false;
        }
        if (_tickets.Any(ticket => ticket.VisitorId == id))
        {
            throw new InvalidOperationException("У посетителя есть билеты. Сначала удалите связанные билеты.");
        }
        _visitors.RemoveAt(index);
        return true;
    }

    public bool RemoveTicket(int id)
    {
        int index = _tickets.FindIndex(item => item.Id == id);
        if (index < 0)
        {
            return false;
        }
        _tickets.RemoveAt(index);
        return true;
    }

    private void ValidateTicketReferences(Ticket ticket)
    {
        if (!_exhibits.Any(item => item.Id == ticket.ExhibitId) ||
            !_visitors.Any(item => item.Id == ticket.VisitorId))
        {
            throw new ArgumentException("Сначала добавьте посетителя и экспонат, указанные в билете.");
        }
    }
}
