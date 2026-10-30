using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
using Stefanescu_Dimitrie_Lab2.ViewModels;

namespace Stefanescu_Dimitrie_Lab2.Pages.Categories;

public class IndexModel(Stefanescu_Dimitrie_Lab2Context context) : PageModel
{
    public IList<Category> Category { get; set; } = new List<Category>();
    public CategoryIndexData CategoryData { get; set; } = new();
    public int CategoryID { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        Category = await context.Category.AsNoTracking()
            .OrderBy(x => x.CategoryName).ThenBy(x => x.ID).ToListAsync();
        CategoryData.Categories = Category;
        if (id.HasValue)
        {
            if (!Category.Any(x => x.ID == id.Value)) return NotFound();
            CategoryID = id.Value;
            CategoryData.Books = await context.Book.AsNoTracking().Include(x => x.Author)
                .Where(x => x.BookCategories.Any(c => c.CategoryID == id.Value))
                .OrderBy(x => x.Title).ThenBy(x => x.ID).ToListAsync();
        }
        return Page();
    }
}
