using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Stefanescu_Dimitrie_Lab2.Models;
public class Member
{
 public int ID { get; set; }
 [StringLength(100)] public string? FirstName { get; set; }
 [StringLength(100)] public string? LastName { get; set; }
 [Display(Name="Address"), StringLength(500)] public string? Adress { get; set; }
 [Required, EmailAddress, StringLength(256)] public string Email { get; set; } = string.Empty;
 [Phone, StringLength(50)] public string? Phone { get; set; }
 [NotMapped, Display(Name="Full Name")] public string FullName => string.Join(" ", new[] { FirstName, LastName }.Where(x => !string.IsNullOrWhiteSpace(x)));
 [NotMapped] public string DisplayName => string.IsNullOrWhiteSpace(FullName) ? Email : FullName;
 public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
}
