using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Stefanescu_Dimitrie_Lab2.Data;
using Stefanescu_Dimitrie_Lab2.Models;
namespace Stefanescu_Dimitrie_Lab2.Areas.Identity.Pages.Account;
[AllowAnonymous]
public class RegisterModel(UserManager<IdentityUser> users, LibraryIdentityContext identity,
 Stefanescu_Dimitrie_Lab2Context library, IWebHostEnvironment environment, IEmailSender emailSender) : PageModel
{
 [BindProperty] public InputModel Input { get; set; } = new();
 public string ReturnUrl { get; set; } = "/";
 public class InputModel
 {
  [Required, EmailAddress, StringLength(256)] public string Email { get; set; } = string.Empty;
  [Required, StringLength(100, MinimumLength=6), DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
  [Required, DataType(DataType.Password), Compare(nameof(Password))] public string ConfirmPassword { get; set; } = string.Empty;
 }
 public void OnGet(string? returnUrl = null) => ReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl! : "/";
 public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
 {
  ReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl! : "/";
  if (!ModelState.IsValid) return Page();
  var email = Input.Email.Trim();
  // Both contexts share one SQL connection and transaction: no partial account/member writes.
  await using var transaction = await identity.Database.BeginTransactionAsync();
  library.Database.SetDbConnection(identity.Database.GetDbConnection());
  await library.Database.UseTransactionAsync(transaction.GetDbTransaction());
  var user = new IdentityUser { UserName = email, Email = email };
  try
  {
   var result = await users.CreateAsync(user, Input.Password);
   if (!result.Succeeded)
   {
    foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
    await transaction.RollbackAsync();
    return Page();
   }
   // Reuse a pre-existing member instead of duplicating their email/contact data.
   if (!await library.Member.AnyAsync(x=>x.Email == email))
   {
    library.Member.Add(new Member { Email = email });
    await library.SaveChangesAsync();
   }
   await transaction.CommitAsync();
  }
  catch (DbUpdateException)
  {
   await transaction.RollbackAsync();
   ModelState.AddModelError(string.Empty, "Înregistrarea nu a putut fi salvată. Verificați dacă emailul este deja folosit și reîncercați.");
   return Page();
  }
  var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await users.GenerateEmailConfirmationTokenAsync(user)));
  var confirmationUrl = Url.Page("/Account/ConfirmEmail", null, new { area="Identity", userId=user.Id, code, returnUrl=ReturnUrl }, Request.Scheme)!;
  if (environment.IsDevelopment()) TempData["ConfirmationUrl"] = confirmationUrl;
  else await emailSender.SendEmailAsync(email, "Confirm your email", $"Confirm your account: {System.Net.WebUtility.HtmlEncode(confirmationUrl)}");
  return RedirectToPage("./RegisterConfirmation");
 }
}
