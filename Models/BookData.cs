namespace Stefanescu_Dimitrie_Lab2.Models;
public class BookData
{
    public IEnumerable<Book> Books { get; set; } = Array.Empty<Book>();
    public IEnumerable<Category> Categories { get; set; } = Array.Empty<Category>();
    public IEnumerable<BookCategory> BookCategories { get; set; } = Array.Empty<BookCategory>();
}
