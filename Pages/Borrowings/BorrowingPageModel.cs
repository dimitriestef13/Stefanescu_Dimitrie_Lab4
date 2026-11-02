using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Pages.Borrowings;
public class BorrowingPageModel(Stefanescu_Dimitrie_Lab2Context context) : PageModel
{
 protected readonly Stefanescu_Dimitrie_Lab2Context Context = context;
 public SelectList Members { get; set; } = default!;
 public SelectList Books { get; set; } = default!;
 protected IQueryable<Borrowing> Query => Context.Borrowing.Include(x => x.Member).Include(x => x.Book).ThenInclude(x => x!.Author);
 protected async Task LoadListsAsync(Borrowing borrowing)
 {
  Members = new SelectList((await Context.Member.AsNoTracking().OrderBy(x=>x.LastName).ThenBy(x=>x.FirstName).ToListAsync()).Select(x=>new { x.ID, Name=x.DisplayName }), "ID", "Name", borrowing.MemberID);
  Books = new SelectList((await Context.Book.AsNoTracking().Include(x=>x.Author).OrderBy(x=>x.Title).ToListAsync()).Select(x=>new { x.ID, Name=x.Title+" - "+(x.Author?.FullName ?? "Fără autor") }), "ID", "Name", borrowing.BookID);
 }
 protected async Task ValidateReferencesAsync(Borrowing b)
 {
  if (!await Context.Member.AnyAsync(x=>x.ID==b.MemberID)) ModelState.AddModelError("Borrowing.MemberID", "Selectați un membru existent.");
  if (!await Context.Book.AnyAsync(x=>x.ID==b.BookID)) ModelState.AddModelError("Borrowing.BookID", "Selectați o carte existentă.");
 }
}
