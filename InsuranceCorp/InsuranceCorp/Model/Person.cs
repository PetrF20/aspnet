using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.Loader;

namespace InsuranceCorp.Model;

public class Person
{
    public int Id { get; set; }

    [MaxLength(250)] //datová anotace informace pro vytvoření DB
    [Display(Name = "Jméno")]
    public string? FirstName { get; set; } //otazník říká, že je pole nepovinné, tvoří nullable type
    [MaxLength(250)]

    public string? LastName { get; set; }

    [MaxLength(525)]
    [EmailAddress]
    public string? Email { get; set; }

    [Display(Name = "Datum narození")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    public Address? Address { get; set; }
    public ICollection<Contract> Constracts { get; set; } = new HashSet<Contract>();
    public override string ToString() => $"{FirstName} {LastName} {Email} {DateOfBirth.ToString("yyyy")} ({Constracts?.Count()}) {Address?.City}";

}
