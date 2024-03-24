using System;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618
public class Member
{
    [Key]
    public int MemberId { get; set; }

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

    [Required(ErrorMessage = "Phone is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string Phone { get; set; }

    [Phone(ErrorMessage = "Invalid cell phone number")]
    public string CellPhone { get; set; }

    [Required(ErrorMessage = "Positive result date is required")]
    [DataType(DataType.Date)]
    public DateTime PositiveResultDate { get; set; }

    [Required(ErrorMessage = "Recovery date is required")]
    [DataType(DataType.Date)]
    public DateTime RecoveryDate { get; set; }
}
#pragma warning restore CS8618
