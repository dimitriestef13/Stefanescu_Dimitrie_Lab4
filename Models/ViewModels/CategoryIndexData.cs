using Stefanescu_Dimitrie_Lab2.Models;

namespace Stefanescu_Dimitrie_Lab2.ViewModels;

public class CategoryIndexData
{
    public IEnumerable<Category> Categories { get; set; } = Array.Empty<Category>();
    public IEnumerable<Book> Books { get; set; } = Array.Empty<Book>();
}
