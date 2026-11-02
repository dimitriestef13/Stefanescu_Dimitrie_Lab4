using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Pages.Members;
public class IndexModel(Stefanescu_Dimitrie_Lab2Context context) : PageModel
{
 private readonly Stefanescu_Dimitrie_Lab2Context Context = context;
 public IList<Member> Items { get; set; } = new List<Member>();
 public async Task OnGetAsync() => Items = await Context.Member.AsNoTracking().ToListAsync();
}
