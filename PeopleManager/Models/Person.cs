using System.ComponentModel.DataAnnotations;

namespace PeopleManager.Models;

public class Person
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Range(0, 150)]
    public int Age { get; set; }

    [Display(Name = "Full Name")]
    public string FullName => $"{FirstName} {LastName}";
}
