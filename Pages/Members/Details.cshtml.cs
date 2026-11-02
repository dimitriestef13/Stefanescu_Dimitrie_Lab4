using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Pages.Members;
public class DetailsModel(Stefanescu_Dimitrie_Lab2Context context) : PageModel
{ private readonly Stefanescu_Dimitrie_Lab2Context Context = context;
 [BindProperty] public Member Member { get; set; } = new();
 public async Task<IActionResult> OnGetAsync(int? id) {
 var item = await Context.Member.AsNoTracking().FirstOrDefaultAsync(x=>x.ID==id);
 if(item is null) return NotFound();
 Member=item;  return Page(); }
 
}
