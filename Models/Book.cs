using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stefanescu_Dimitrie_Lab2.Models
{
    public class Book
    {
        public int ID { get; set; }
        public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();

        [Display(Name = "Categories")]
        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
        
        [Display(Name = "Book Title")]
        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        public int? AuthorID { get; set; }

        public Authors? Author { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        [Range(typeof(decimal), "0.01", "500", ParseLimitsInInvariantCulture = true)]
        public decimal Price { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishingDate { get; set; }
        public int? PublisherID { get; set; }

        public Publisher? Publisher { get; set; }

    }
}
