using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
public interface IMemberValidationService
{
    List<ValidationResult> ValidateMember(Member member);
}
public class MemberValidationService : IMemberValidationService
{
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
        // Check for maximum vaccinations
        if (member.Vaccinations != null && member.Vaccinations.Count > 4)
        {
            yield return new ValidationResult("A member cannot have more than 4 vaccinations.");
        }
    }
}

