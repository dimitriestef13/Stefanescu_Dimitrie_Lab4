using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Pages.Borrowings;
public class IndexModel(Stefanescu_Dimitrie_Lab2Context context) : BorrowingPageModel(context)
{
 
 public IList<Borrowing> Items { get; set; } = new List<Borrowing>();
 public async Task OnGetAsync() => Items = await Query.AsNoTracking().ToListAsync();
}
