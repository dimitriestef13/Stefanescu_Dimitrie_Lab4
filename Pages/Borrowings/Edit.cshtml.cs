using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Pages.Borrowings;
public class EditModel(Stefanescu_Dimitrie_Lab2Context context) : BorrowingPageModel(context)
{ 
 [BindProperty] public Borrowing Borrowing { get; set; } = new();
 public async Task<IActionResult> OnGetAsync(int? id) {
 var item = await Query.AsNoTracking().FirstOrDefaultAsync(x=>x.ID==id);
 if(item is null) return NotFound();
 Borrowing=item; await LoadListsAsync(Borrowing); return Page(); }
 public async Task<IActionResult> OnPostAsync(int? id) {
 if(id != Borrowing.ID) return NotFound();
 await ValidateReferencesAsync(Borrowing);
 if(!ModelState.IsValid) { await LoadListsAsync(Borrowing); return Page(); }
 var item = await Context.Borrowing.FindAsync(id);
 if(item is null) return NotFound();
 item.MemberID = Borrowing.MemberID;
item.BookID = Borrowing.BookID;
item.ReturnDate = Borrowing.ReturnDate;
 try { await Context.SaveChangesAsync(); }
 catch(DbUpdateConcurrencyException) { return NotFound(); }
 catch(DbUpdateException) { ModelState.AddModelError("", "Datele nu au putut fi salvate. Verificați valorile și reîncercați."); await LoadListsAsync(Borrowing); return Page(); }
 return RedirectToPage("./Index"); }
}
