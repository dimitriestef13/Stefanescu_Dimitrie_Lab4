using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Stefanescu_Dimitrie_Lab2.Data;
public class LibraryIdentityContext(DbContextOptions<LibraryIdentityContext> options) : IdentityDbContext<IdentityUser>(options);
