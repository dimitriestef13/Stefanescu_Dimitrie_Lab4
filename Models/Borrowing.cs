using System.ComponentModel.DataAnnotations;
namespace Stefanescu_Dimitrie_Lab2.Models;
public class Borrowing
{
 public int ID { get; set; }
 [Display(Name="Member")] public int? MemberID { get; set; }
 public Member? Member { get; set; }
 [Display(Name="Book")] public int? BookID { get; set; }
 public Book? Book { get; set; }
 [Required, DataType(DataType.Date), Display(Name="Return Date")] public DateTime ReturnDate { get; set; } = DateTime.Today.AddDays(14);
}
