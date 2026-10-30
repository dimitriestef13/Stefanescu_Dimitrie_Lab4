using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Pages.BookPages;

public class IndexModel(Stefanescu_Dimitrie_Lab2Context context) : PageModel
{
    public IList<Book> Book { get; set; } = new List<Book>();
    public BookData BookD { get; set; } = new();
    public int BookID { get; set; }
    public int CategoryID { get; set; }
    public string TitleSort { get; set; } = "title_desc";
    public string AuthorSort { get; set; } = "author";
    public string CurrentFilter { get; set; } = string.Empty;
    public string CurrentSort { get; set; } = string.Empty;

    public async Task OnGetAsync(int? id, int? categoryID, string? sortOrder, string? searchString)
    {
        CurrentSort = sortOrder is "title_desc" or "author" or "author_desc" ? sortOrder : "";
        TitleSort = CurrentSort == "" ? "title_desc" : "";
        AuthorSort = CurrentSort == "author" ? "author_desc" : "author";
        CurrentFilter = searchString?.Trim() ?? "";
        var query = context.Book.Include(x => x.Author).Include(x => x.Publisher)
            .Include(x => x.BookCategories).ThenInclude(x => x.Category).AsNoTracking();
        if (!string.IsNullOrEmpty(CurrentFilter))
        {
            query = query.Where(x => x.Title.Contains(CurrentFilter) ||
                (x.Author != null && (x.Author.FirstName.Contains(CurrentFilter) ||
                                     x.Author.LastName.Contains(CurrentFilter))));
        }
        // FullName is computed, so sort by its SQL-translatable equivalent.
        var sorted = CurrentSort switch
        {
            "title_desc" => query.OrderByDescending(x => x.Title).ThenBy(x => x.ID),
            "author" => query.OrderBy(x => x.Author == null ? "" :
                (x.Author.FirstName + " " + x.Author.LastName).Trim()).ThenBy(x => x.Title).ThenBy(x => x.ID),
            "author_desc" => query.OrderByDescending(x => x.Author == null ? "" :
                (x.Author.FirstName + " " + x.Author.LastName).Trim()).ThenBy(x => x.Title).ThenBy(x => x.ID),
            _ => query.OrderBy(x => x.Title).ThenBy(x => x.ID)
        };
        Book = await sorted.ToListAsync();
        BookD.Books = Book;
        BookD.BookCategories = Book.SelectMany(x => x.BookCategories);
        CategoryID = categoryID ?? 0;
        var selected = Book.FirstOrDefault(x => x.ID == id);
        if (selected is not null)
        {
            BookID = selected.ID;
            BookD.Categories = selected.BookCategories.Select(x => x.Category);
        }
    }
}
