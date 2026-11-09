using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Pages.BookPages;
[Authorize(Roles = "Admin")]
public class CreateModel(Stefanescu_Dimitrie_Lab2Context context) : BookCategoriesPageModel
{
    [BindProperty]
    public Book Book { get; set; } = new();
    public IActionResult OnGet()
    {
        PopulateLookups(context, Book);
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(string[]? selectedCategories)
    {
        // Bind scalar fields only; category links are constructed from validated IDs.
        var newBook = new Book { Title = Book.Title, AuthorID = Book.AuthorID, Price = Book.Price,
            PublishingDate = Book.PublishingDate, PublisherID = Book.PublisherID };
        UpdateBookCategories(context, selectedCategories, newBook);
        Book = newBook;
        if (!ModelState.IsValid)
        {
            PopulateLookups(context, Book);
            return Page();
        }
        context.Book.Add(Book);
        await context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}
