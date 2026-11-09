using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Pages.BookPages;
[Authorize(Roles = "Admin")]
public class EditModel(Stefanescu_Dimitrie_Lab2Context context) : BookCategoriesPageModel
{
    [BindProperty]
    public Book Book { get; set; } = new();
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return NotFound();
        var book = await context.Book.Include(x => x.Author).Include(x => x.Publisher)
            .Include(x => x.BookCategories).ThenInclude(x => x.Category).AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
        if (book is null) return NotFound();
        Book = book;
        PopulateLookups(context, Book);
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int? id, string[]? selectedCategories)
    {
        if (id is null || id != Book.ID) return NotFound();
        var book = await context.Book.Include(x => x.BookCategories).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.ID == id);
        if (book is null) return NotFound();
        await TryUpdateModelAsync(book, "Book", x => x.Title, x => x.AuthorID, x => x.Price, x => x.PublishingDate, x => x.PublisherID);
        UpdateBookCategories(context, selectedCategories, book);
        if (!ModelState.IsValid)
        {
            Book = book;
            PopulateLookups(context, Book);
            return Page();
        }
        try { await context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!await context.Book.AnyAsync(x => x.ID == id)) return NotFound();
            throw;
        }
        return RedirectToPage("./Index");
    }
}

