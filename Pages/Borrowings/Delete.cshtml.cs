using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Pages.Borrowings;
public class DeleteModel(Stefanescu_Dimitrie_Lab2Context context) : BorrowingPageModel(context)
{ 
 [BindProperty] public Borrowing Borrowing { get; set; } = new();
 public async Task<IActionResult> OnGetAsync(int? id) {
 var item = await Query.AsNoTracking().FirstOrDefaultAsync(x=>x.ID==id);
 if(item is null) return NotFound();
 Borrowing=item; await LoadListsAsync(Borrowing); return Page(); }
 public async Task<IActionResult> OnPostAsync(int? id) {
 var item=await Context.Borrowing.FindAsync(id); if(item is null) return NotFound();
 Context.Borrowing.Remove(item); await Context.SaveChangesAsync(); return RedirectToPage("./Index"); }
}
