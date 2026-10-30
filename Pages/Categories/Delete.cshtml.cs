using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Pages.Categories;
public class DeleteModel(Stefanescu_Dimitrie_Lab2Context context) : PageModel
{
    [BindProperty]
    public Category Category { get; set; } = new();
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return NotFound();
        var category = await context.Category.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
        if (category is null) return NotFound();
        Category = category;
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null) return NotFound();
        var category = await context.Category.FindAsync(id);
        if (category is null) return NotFound();
        context.Category.Remove(category);
        await context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}
