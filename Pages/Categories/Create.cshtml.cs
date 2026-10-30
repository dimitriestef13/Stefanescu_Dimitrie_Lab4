using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Pages.Categories;
public class CreateModel(Stefanescu_Dimitrie_Lab2Context context) : PageModel
{
    [BindProperty]
    public Category Category { get; set; } = new();
    public void OnGet() { }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        context.Category.Add(new Category { CategoryName = Category.CategoryName });
        await context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}
