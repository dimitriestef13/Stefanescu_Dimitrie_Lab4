using Microsoft.EntityFrameworkCore;

public class Stefanescu_Dimitrie_Lab2Context(DbContextOptions<Stefanescu_Dimitrie_Lab2Context> options) : DbContext(options)
{
    public DbSet<Stefanescu_Dimitrie_Lab2.Models.Book> Book { get; set; } = default!;

    public DbSet<Stefanescu_Dimitrie_Lab2.Models.Publisher> Publisher { get; set; } = default!;

    public DbSet<Stefanescu_Dimitrie_Lab2.Models.Authors> Authors { get; set; } = default!;

    public DbSet<Stefanescu_Dimitrie_Lab2.Models.Category> Category { get; set; } = default!;
    public DbSet<Stefanescu_Dimitrie_Lab2.Models.BookCategory> BookCategory { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Stefanescu_Dimitrie_Lab2.Models.BookCategory>(entity =>
        {
            entity.HasIndex(x => new { x.BookID, x.CategoryID }).IsUnique();
            entity.HasOne(x => x.Book).WithMany(x => x.BookCategories).HasForeignKey(x => x.BookID);
            entity.HasOne(x => x.Category).WithMany(x => x.BookCategories).HasForeignKey(x => x.CategoryID);
        });
    }
}
