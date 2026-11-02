using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Pages.Members;
public class CreateModel(Stefanescu_Dimitrie_Lab2Context context) : PageModel
{ private readonly Stefanescu_Dimitrie_Lab2Context Context = context;
 [BindProperty] public Member Member { get; set; } = new();
 public async Task<IActionResult> OnGetAsync() {  return Page(); }
 public async Task<IActionResult> OnPostAsync(int? id) {
 Member.ID = 0;
 Member.Email = Member.Email.Trim();
 if (await Context.Member.AnyAsync(x => x.Email == Member.Email && x.ID != Member.ID)) ModelState.AddModelError("Member.Email", "Există deja un membru cu acest email.");
 if(!ModelState.IsValid) {  return Page(); }
 Context.Member.Add(Member);
 try { await Context.SaveChangesAsync(); }
 catch(DbUpdateConcurrencyException) { return NotFound(); }
 catch(DbUpdateException) { ModelState.AddModelError("", "Datele nu au putut fi salvate. Verificați valorile și reîncercați.");  return Page(); }
 return RedirectToPage("./Index"); }
}
