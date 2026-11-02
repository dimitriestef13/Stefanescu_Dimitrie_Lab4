using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Pages.Members;
public class EditModel(Stefanescu_Dimitrie_Lab2Context context) : PageModel
{ private readonly Stefanescu_Dimitrie_Lab2Context Context = context;
 [BindProperty] public Member Member { get; set; } = new();
 public async Task<IActionResult> OnGetAsync(int? id) {
 var item = await Context.Member.AsNoTracking().FirstOrDefaultAsync(x=>x.ID==id);
 if(item is null) return NotFound();
 Member=item;  return Page(); }
 public async Task<IActionResult> OnPostAsync(int? id) {
 if(id != Member.ID) return NotFound();
 Member.Email = Member.Email.Trim();
 if (await Context.Member.AnyAsync(x => x.Email == Member.Email && x.ID != Member.ID)) ModelState.AddModelError("Member.Email", "Există deja un membru cu acest email.");
 if(!ModelState.IsValid) {  return Page(); }
 var item = await Context.Member.FindAsync(id);
 if(item is null) return NotFound();
 item.FirstName = Member.FirstName;
item.LastName = Member.LastName;
item.Adress = Member.Adress;
item.Email = Member.Email;
item.Phone = Member.Phone;
 try { await Context.SaveChangesAsync(); }
 catch(DbUpdateConcurrencyException) { return NotFound(); }
 catch(DbUpdateException) { ModelState.AddModelError("", "Datele nu au putut fi salvate. Verificați valorile și reîncercați.");  return Page(); }
 return RedirectToPage("./Index"); }
}
