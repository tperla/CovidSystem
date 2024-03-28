using CovidSystem.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
public interface IValidationService
{
    List<ValidationResult> ValidateMember(Member member);
    List<ValidationResult> ValidateVaccination(Vaccination vaccination);
}
public class ValidationService : IValidationService
{
    private readonly CovidDbContext _context;
    public ValidationService(CovidDbContext context)
    {
        _context = context;
    }
    public List<ValidationResult> ValidateMember(Member member)
    {
        try
        {
            var validationResults = GetMemberValidators(member);
            var validationErrors = new List<ValidationResult>();

            foreach (var validationResult in validationResults)
            {
                validationErrors.Add(new ValidationResult(validationResult.ErrorMessage));
            }
            return validationErrors;
        }
        catch (Exception ex)
        {
            // Print the error message directly
            Console.WriteLine($"Error in ValidateMember method: {ex.Message}");
            return new List<ValidationResult>();
        }
    }
    public List<ValidationResult> ValidateVaccination(Vaccination vaccination)
    {
        try
        {
            var validationResults = GetVaccinationValidators(vaccination);
            var validationErrors = new List<ValidationResult>();

            foreach (var validationResult in validationResults)
            {
                validationErrors.Add(new ValidationResult(validationResult.ErrorMessage));
            }
            return validationErrors;
        }
        catch (Exception ex)
        {
            // Print the error message directly
            Console.WriteLine($"Error in ValidateVaccination method: {ex.Message}");
            return new List<ValidationResult>();
        }
    }

    private IEnumerable<ValidationResult> GetMemberValidators(Member member)
    {
        if (string.IsNullOrEmpty(member.Phone) && string.IsNullOrEmpty(member.CellPhone))
        {
            yield return new ValidationResult("Please fill in at least one phone number.");
        }
        if (!member.Address.Contains(","))
        {
            yield return new ValidationResult("Address should contain city, street, and number separated by commas.");
        }
        if (member.BirthDate > DateTime.Today)
        {
            yield return new ValidationResult("Birth date cannot be in the future.");
        }
        if (member.PositiveResultDate.HasValue && member.PositiveResultDate.Value > DateTime.Today)
        {
            yield return new ValidationResult("Positive result date cannot be in the future.");
        }
        if (member.RecoveryDate.HasValue && (member.RecoveryDate.Value > DateTime.Today || member.RecoveryDate.Value < member.PositiveResultDate))
        {
            yield return new ValidationResult("Invalid recovery date.");
        }
        if (member.RecoveryDate.HasValue && !member.PositiveResultDate.HasValue)
        {
            yield return new ValidationResult("If recovery date is provided, positive result date must also be provided.");
        }
        // Check for duplicate MemberId
        if (_context != null && _context.Members.Any(m => m.MemberId == member.MemberId))
        {
            yield return new ValidationResult("Member ID already exists.");
        }
        // Check for maximum vaccinations
        if (member.Vaccinations != null && member.Vaccinations.Count > 4)
        {
            yield return new ValidationResult("A member cannot have more than 4 vaccinations.");
        }
    }
    private IEnumerable<ValidationResult> GetVaccinationValidators(Vaccination vaccination)
    {
        if (vaccination.VaccinationDate > DateTime.Today)
        {
            yield return new ValidationResult("Vaccination date cannot be in the future.");
        }

        // Check if the context is not null and the MemberId exists
        if (_context == null || !_context.Members.Any(m => m.MemberId == vaccination.MemberId))
        {
            yield return new ValidationResult("Member ID does not exist.");
        }

        // Check if the context is not null and the ManufacturerId exists
        if (_context == null || !_context.Manufacturers.Any(manufacturer => manufacturer.ManufacturerId == vaccination.ManufacturerId))
        {
            yield return new ValidationResult("Manufacturer ID does not exist.");
        }
    }
}

