using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Stefanescu_Dimitrie_Lab2.Areas.Identity.Pages.Account;
[AllowAnonymous]
public class RegisterConfirmationModel(IWebHostEnvironment environment) : PageModel
{
 public string? ConfirmationUrl { get; set; }
 public void OnGet() { if (environment.IsDevelopment()) ConfirmationUrl = TempData["ConfirmationUrl"] as string; }
}
