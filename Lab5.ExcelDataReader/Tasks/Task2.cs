using Lab5.ExcelLibrary.Models;

namespace Lab5.ExcelLibrary.Tasks;

internal static class Task2
{
    public static decimal Execute(MuseumContext context)
    {
        return RenaissanceFilter.JuneTickets(context).Sum(ticket => ticket.Price);
    }
}
