using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Stefanescu_Dimitrie_Lab2.Areas.Identity.Pages.Account;
public class LogoutModel(SignInManager<IdentityUser> signInManager) : PageModel
{
 public void OnGet() { }
 public async Task<IActionResult> OnPostAsync(string? returnUrl=null) {
  await signInManager.SignOutAsync();
  return LocalRedirect(Url.IsLocalUrl(returnUrl) ? returnUrl! : "/");
 }
}
