using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Models;

namespace Stefanescu_Dimitrie_Lab2.Pages.Members;

public class EditModel(Stefanescu_Dimitrie_Lab2Context context) : PageModel
{
    public Member Member { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var member = await context.Member.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
        if (member is null)
            return NotFound();

        Member = member;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync([FromQuery] int? id)
    {
        if (id is null)
            return NotFound();

        var member = await context.Member.FindAsync(id);
        if (member is null)
            return NotFound();

        Member = member;
        if (!await TryUpdateModelAsync(Member, nameof(Member), x => x.FirstName, x => x.LastName, x => x.Adress, x => x.Phone))
            return Page();

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(string.Empty, "Datele nu au putut fi salvate. Verificați valorile și reîncercați.");
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
