using Lab5.ExcelLibrary.Models;

namespace Lab5.ExcelLibrary.Tasks;

internal static class Task1
{
    public static List<Exhibit> Execute(MuseumContext context)
    {
        return context.Exhibits
            .Where(RenaissanceFilter.IsRenaissanceExhibit)
            .OrderBy(exhibit => exhibit.Id)
            .ToList();
    }
}
