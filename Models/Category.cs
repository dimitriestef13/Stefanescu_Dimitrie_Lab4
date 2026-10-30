using System.ComponentModel.DataAnnotations;
namespace Stefanescu_Dimitrie_Lab2.Models;
public class Category
{
    public int ID { get; set; }
    [Required, Display(Name = "Category Name")]
    public string CategoryName { get; set; } = string.Empty;
    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
}
