using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Stefanescu_Dimitrie_Lab2.Models;
public class BookCategoriesPageModel : PageModel
{
    public List<AssignedCategoryData> AssignedCategoryDataList { get; set; } = new();

    public void PopulateAssignedCategoryData(Stefanescu_Dimitrie_Lab2Context context, Book book)
    {
        var assigned = book.BookCategories.Select(x => x.CategoryID).ToHashSet();
        AssignedCategoryDataList = context.Category.OrderBy(x => x.CategoryName).ToList()
            .Select(x => new AssignedCategoryData { CategoryID = x.ID, Name = x.CategoryName, Assigned = assigned.Contains(x.ID) }).ToList();
    }

    public void UpdateBookCategories(Stefanescu_Dimitrie_Lab2Context context, string[]? selectedCategories, Book bookToUpdate)
    {
        var selected = new HashSet<int>();
        foreach (var value in selectedCategories ?? Array.Empty<string>())
        {
            if (int.TryParse(value, out var id)) selected.Add(id);
            else ModelState.AddModelError("", "Invalid category.");
        }
        var valid = context.Category.Select(x => x.ID).ToHashSet();
        if (!selected.IsSubsetOf(valid)) ModelState.AddModelError("", "A selected category no longer exists.");
        selected.IntersectWith(valid);
        foreach (var link in bookToUpdate.BookCategories.Where(x => !selected.Contains(x.CategoryID)).ToList())
        {
            bookToUpdate.BookCategories.Remove(link);
            if (link.ID != 0) context.BookCategory.Remove(link);
        }
        var existing = bookToUpdate.BookCategories.Select(x => x.CategoryID).ToHashSet();
        foreach (var id in selected.Except(existing))
            bookToUpdate.BookCategories.Add(new BookCategory { CategoryID = id });
    }

    protected void PopulateLookups(Stefanescu_Dimitrie_Lab2Context context, Book book)
    {
        ViewData["AuthorID"] = new SelectList(context.Authors.OrderBy(x => x.LastName)
            .Select(x => new { x.ID, FullName = x.FirstName + " " + x.LastName }), "ID", "FullName", book.AuthorID);
        ViewData["PublisherID"] = new SelectList(context.Publisher.OrderBy(x => x.PublisherName), "ID", "PublisherName", book.PublisherID);
        PopulateAssignedCategoryData(context, book);
    }
}
