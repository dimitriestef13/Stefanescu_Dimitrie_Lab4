using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stefanescu_Dimitrie_Lab2.Data;

var grantAdmin = args.FirstOrDefault() == "--grant-admin";
if (grantAdmin && (args.Length != 2 || string.IsNullOrWhiteSpace(args[1])))
{
    Console.Error.WriteLine("Usage: --grant-admin <existing-account-email>");
    Environment.ExitCode = 1;
    return;
}

var builder = WebApplication.CreateBuilder(grantAdmin ? [] : args);
var connectionString = builder.Configuration.GetConnectionString("Stefanescu_Dimitrie_Lab2Context") ?? throw new InvalidOperationException("Connection string 'Stefanescu_Dimitrie_Lab2Context' not found.");

builder.Services.AddDbContext<Stefanescu_Dimitrie_Lab2Context>(options => options.UseSqlServer(connectionString));

builder.Services.AddDbContext<LibraryIdentityContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<LibraryIdentityContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/BookPages/Index", "/BookPages");
    options.Conventions.AddPageRoute("/BookPages/Index", "/BookPages/Index");
    options.Conventions.AuthorizeFolder("/BookPages");
    options.Conventions.AllowAnonymousToPage("/BookPages/Index");
    options.Conventions.AllowAnonymousToPage("/BookPages/Details");
    options.Conventions.AuthorizeFolder("/Members", "AdminPolicy");
    options.Conventions.AuthorizeFolder("/PublisherPages", "AdminPolicy");
    options.Conventions.AuthorizeFolder("/Categories", "AdminPolicy");
});

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await LibraryRoles.EnsureCreatedAsync(roles);

    if (grantAdmin)
    {
        var users = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        Environment.ExitCode = await LibraryRoles.GrantAdminAsync(users, args[1].Trim());
    }
}

if (grantAdmin)
{
    await app.DisposeAsync();
    return;
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

