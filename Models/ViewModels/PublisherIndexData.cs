using Stefanescu_Dimitrie_Lab2.Models;

namespace Stefanescu_Dimitrie_Lab2.ViewModels;

public class PublisherIndexData
{
    public IEnumerable<Publisher> Publishers { get; set; } = Array.Empty<Publisher>();
    public IEnumerable<Book> Books { get; set; } = Array.Empty<Book>();
}
