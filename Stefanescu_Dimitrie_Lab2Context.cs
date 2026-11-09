using Microsoft.EntityFrameworkCore;

public class Stefanescu_Dimitrie_Lab2Context(DbContextOptions<Stefanescu_Dimitrie_Lab2Context> options) : DbContext(options)
{
    public DbSet<Stefanescu_Dimitrie_Lab2.Models.Book> Book { get; set; } = default!;

    public DbSet<Stefanescu_Dimitrie_Lab2.Models.Publisher> Publisher { get; set; } = default!;

    public DbSet<Stefanescu_Dimitrie_Lab2.Models.Authors> Authors { get; set; } = default!;

    public DbSet<Stefanescu_Dimitrie_Lab2.Models.Category> Category { get; set; } = default!;
    public DbSet<Stefanescu_Dimitrie_Lab2.Models.BookCategory> BookCategory { get; set; } = default!;

    public DbSet<Stefanescu_Dimitrie_Lab2.Models.Member> Member { get; set; } = default!;
    public DbSet<Stefanescu_Dimitrie_Lab2.Models.Borrowing> Borrowing { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Form validation can be tightened without narrowing columns that hold existing data.
        modelBuilder.Entity<Stefanescu_Dimitrie_Lab2.Models.Book>().Property(x => x.Title).Metadata.SetMaxLength(null);
        modelBuilder.Entity<Stefanescu_Dimitrie_Lab2.Models.Member>(entity =>
        {
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.FirstName).HasMaxLength(100);
            entity.Property(x => x.LastName).HasMaxLength(100);
            entity.Property(x => x.Adress).HasMaxLength(500);
        });
        modelBuilder.Entity<Stefanescu_Dimitrie_Lab2.Models.Borrowing>()
            .HasOne(x => x.Member).WithMany(x => x.Borrowings).HasForeignKey(x => x.MemberID).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Stefanescu_Dimitrie_Lab2.Models.Borrowing>()
            .HasOne(x => x.Book).WithMany(x => x.Borrowings).HasForeignKey(x => x.BookID).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Stefanescu_Dimitrie_Lab2.Models.BookCategory>(entity =>
        {
            entity.HasIndex(x => new { x.BookID, x.CategoryID }).IsUnique();
            entity.HasOne(x => x.Book).WithMany(x => x.BookCategories).HasForeignKey(x => x.BookID);
            entity.HasOne(x => x.Category).WithMany(x => x.BookCategories).HasForeignKey(x => x.CategoryID);
        });
    }
}
