using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#pragma warning disable CS8618
// This class represents a Member entity in the system.
public class Member
{
    // Primary key for the Member entity, consisting of exactly 9 digits.
    [Key]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "Member Id must contain exactly 9 digits.")]
    [Required(ErrorMessage = "Member Id is required")]
    public string MemberId { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Address is required")]
    [MaxLength(100)]
    public string Address { get; set; }

    [Required(ErrorMessage = "Birth date is required")]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    public string? Phone { get; set; } = null;

    [Phone(ErrorMessage = "Invalid cell phone number")]
    public string? CellPhone { get; set; } = null;

    [DataType(DataType.Date)]
    public DateTime? PositiveResultDate { get; set; } = null;

    [DataType(DataType.Date)]
    public DateTime? RecoveryDate { get; set; } = null;

    // Hash of the member's image (not serialized).
    [JsonIgnore]
    public string? ImageHash { get; set; } = null;

    // Navigation property for vaccinations
    public ICollection<Vaccination>? Vaccinations { get; set; } = null;
}
#pragma warning restore CS8618
