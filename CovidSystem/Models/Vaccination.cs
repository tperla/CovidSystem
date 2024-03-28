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
    public DateTime VaccinationDate { get; set; }

    // Navigation property for member
    [JsonIgnore]
    public Member? Member { get; set; } = null;
    [JsonIgnore]
    public Manufacturer? Manufacturer { get; set; } = null;
}
#pragma warning restore CS8618

