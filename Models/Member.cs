using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Stefanescu_Dimitrie_Lab2.Models;
public class Member
{
 public int ID { get; set; }
 [RegularExpression(@"^[A-Z]+[a-zA-Z\s-]*$", ErrorMessage = "Prenumele trebuie sa inceapa cu majuscula (ex. Ana sau Ana Maria sau Ana-Maria)")]
 [StringLength(30, MinimumLength = 3, ErrorMessage = "Prenumele trebuie să aibă între 3 și 30 de caractere.")]
 public string? FirstName { get; set; }
 [RegularExpression(@"^[A-Z]+[a-z\s]*$", ErrorMessage = "Numele trebuie să înceapă cu majusculă și să conțină doar litere și spații.")]
 [StringLength(30, MinimumLength = 3, ErrorMessage = "Numele trebuie să aibă între 3 și 30 de caractere.")]
 public string? LastName { get; set; }
 [Display(Name = "Address")]
 [StringLength(70, ErrorMessage = "Adresa poate avea cel mult 70 de caractere.")]
 public string? Adress { get; set; }
 [Required, EmailAddress, StringLength(256)] public string Email { get; set; } = string.Empty;
 [DataType(DataType.PhoneNumber)]
 [StringLength(50)]
 [RegularExpression(@"^0[0-9]{3}[-. ]?[0-9]{3}[-. ]?[0-9]{3}$", ErrorMessage = "Telefonul trebuie sa fie de forma '0722-123-123' sau '0722.123.123' sau '0722 123 123'")]
 public string? Phone { get; set; }
 [NotMapped, Display(Name="Full Name")] public string FullName => string.Join(" ", new[] { FirstName, LastName }.Where(x => !string.IsNullOrWhiteSpace(x)));
 [NotMapped] public string DisplayName => string.IsNullOrWhiteSpace(FullName) ? Email : FullName;
 public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
}
