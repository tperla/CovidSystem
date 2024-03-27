using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CovidSystem.DbContexts;

#pragma warning disable CS8618
// This class represents a Vaccination entity in the system.
public class Vaccination
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int VaccinationId { get; set; }

    [Required(ErrorMessage = "Member ID is required")]
    public string MemberId { get; set; }

    [Required(ErrorMessage = "Manufacturer ID is required")]
    public int ManufacturerId { get; set; }

    [Required(ErrorMessage = "Vaccination date is required")]
    [DataType(DataType.Date)]
    [ValidateDateInPast(ErrorMessage = "Vaccination date must be in the past")]
    public DateTime VaccinationDate { get; set; }

    // Navigation property for member
    [JsonIgnore]
    public Member? Member { get; set; } = null;
    [JsonIgnore]
    public Manufacturer? Manufacturer { get; set; } = null;
}
// Custom validation attribute to ensure the date is in the past.
public class ValidateDateInPastAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime date)
        {
            return date <= DateTime.Today;
        }
        return false;
    }
}
#pragma warning restore CS8618

