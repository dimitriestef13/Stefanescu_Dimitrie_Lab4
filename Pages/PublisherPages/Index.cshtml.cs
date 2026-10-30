using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;
using Stefanescu_Dimitrie_Lab2.ViewModels;

namespace Stefanescu_Dimitrie_Lab2.Pages.PublisherPages;

public class IndexModel(Stefanescu_Dimitrie_Lab2Context context) : PageModel
{
    public IList<Publisher> Publisher { get; set; } = new List<Publisher>();
    public PublisherIndexData PublisherData { get; set; } = new();
    public int PublisherID { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        Publisher = await context.Publisher.AsNoTracking()
            .Include(x => x.Books!).ThenInclude(x => x.Author)
            .OrderBy(x => x.PublisherName).ThenBy(x => x.ID).ToListAsync();
        PublisherData.Publishers = Publisher;
        if (id.HasValue)
        {
            var selected = Publisher.FirstOrDefault(x => x.ID == id.Value);
            if (selected is null) return NotFound();
            PublisherID = selected.ID;
            PublisherData.Books = (selected.Books ?? Array.Empty<Book>())
                .OrderBy(x => x.Title).ThenBy(x => x.ID);
        }
        return Page();
    }
}
