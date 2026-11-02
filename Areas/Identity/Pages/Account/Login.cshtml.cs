using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Stefanescu_Dimitrie_Lab2.Areas.Identity.Pages.Account;
[AllowAnonymous]
public class LoginModel(SignInManager<IdentityUser> signInManager) : PageModel
{
 [BindProperty] public InputModel Input { get; set; } = new();
 public string ReturnUrl { get; set; } = "/";
 public class InputModel {
  [Required, EmailAddress] public string Email { get; set; } = string.Empty;
  [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
  [Display(Name="Remember me")] public bool RememberMe { get; set; }
 }
 public void OnGet(string? returnUrl=null) => ReturnUrl=Url.IsLocalUrl(returnUrl)?returnUrl!:"/";
 public async Task<IActionResult> OnPostAsync(string? returnUrl=null) {
  OnGet(returnUrl);
  if (!ModelState.IsValid) return Page();
  var result = await signInManager.PasswordSignInAsync(Input.Email.Trim(), Input.Password, Input.RememberMe, lockoutOnFailure:true);
  if(result.Succeeded) return LocalRedirect(ReturnUrl);
  if(result.RequiresTwoFactor) return RedirectToPage("./LoginWith2fa", new { ReturnUrl, Input.RememberMe });
  ModelState.AddModelError(string.Empty, result.IsLockedOut ? "Cont blocat temporar." : result.IsNotAllowed ? "Confirmați mai întâi adresa de email." : "Email sau parolă incorectă.");
  return Page();
 }
}
