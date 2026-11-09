using Microsoft.AspNetCore.Identity;

namespace Stefanescu_Dimitrie_Lab2.Data;

internal static class LibraryRoles
{
    internal static async Task EnsureCreatedAsync(RoleManager<IdentityRole> roles)
    {
        foreach (var name in new[] { "Admin", "User" })
        {
            if (await roles.RoleExistsAsync(name))
                continue;

            var result = await roles.CreateAsync(new IdentityRole(name));
            if (!result.Succeeded && !await roles.RoleExistsAsync(name))
            {
                var errors = string.Join(", ", result.Errors.Select(error => error.Code));
                throw new InvalidOperationException($"Could not create the {name} role ({errors}).");
            }
        }
    }

    internal static async Task<int> GrantAdminAsync(UserManager<IdentityUser> users, string email)
    {
        var user = await users.FindByEmailAsync(email);
        if (user is null || !await users.IsEmailConfirmedAsync(user))
        {
            Console.Error.WriteLine("An existing account with a confirmed email is required.");
            return 1;
        }

        if (!await users.IsInRoleAsync(user, "Admin"))
        {
            var result = await users.AddToRoleAsync(user, "Admin");
            if (!result.Succeeded)
            {
                Console.Error.WriteLine(string.Join(", ", result.Errors.Select(error => error.Code)));
                return 1;
            }
        }

        Console.WriteLine("Admin role verified. Sign out and sign in again to refresh the session.");
        return 0;
    }
}
