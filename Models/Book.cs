using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stefanescu_Dimitrie_Lab2.Models
{
    public class Book
    {
        public int ID { get; set; }

        [Display(Name = "Categories")]
        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
        
        [Display(Name = "Book Title")]
        public string Title { get; set; } = string.Empty;
        public int? AuthorID { get; set; }

        public Authors? Author { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishingDate { get; set; }
        public int? PublisherID { get; set; }

        public Publisher? Publisher { get; set; }

    }
}
