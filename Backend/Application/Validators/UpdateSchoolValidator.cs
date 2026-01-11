using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class UpdateSchoolValidator : AbstractValidator<UpdateSchoolDto>
{
    public UpdateSchoolValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Valid school ID is required");

        RuleFor(x => x.SchoolName)
            .NotEmpty()
            .WithMessage("School name is required")
            .MaximumLength(200)
            .WithMessage("School name cannot exceed 200 characters");

        RuleFor(x => x.SchoolCode)
            .NotEmpty()
            .WithMessage("School code is required")
            .MaximumLength(50)
            .WithMessage("School code cannot exceed 50 characters");

        RuleFor(x => x.District)
            .NotEmpty()
            .WithMessage("District is required")
            .MaximumLength(100)
            .WithMessage("District cannot exceed 100 characters");

        RuleFor(x => x.Block)
            .NotEmpty()
            .WithMessage("Block is required")
            .MaximumLength(100)
            .WithMessage("Block cannot exceed 100 characters");

        RuleFor(x => x.Village)
            .NotEmpty()
            .WithMessage("Village is required")
            .MaximumLength(100)
            .WithMessage("Village cannot exceed 100 characters");

        RuleFor(x => x.SchoolType)
            .NotEmpty()
            .WithMessage("School type is required")
            .Must(x => new[] { "Primary", "Middle", "High", "Senior Secondary" }.Contains(x))
            .WithMessage("School type must be Primary, Middle, High, or Senior Secondary");

        RuleFor(x => x.ManagementType)
            .NotEmpty()
            .WithMessage("Management type is required")
            .Must(x => new[] { "Government", "Aided", "Private" }.Contains(x))
            .WithMessage("Management type must be Government, Aided, or Private");

        RuleFor(x => x.TotalStudents)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total students cannot be negative");

        RuleFor(x => x.TotalTeachers)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total teachers cannot be negative");

        RuleFor(x => x.PrincipalName)
            .NotEmpty()
            .WithMessage("Principal name is required")
            .MaximumLength(100)
            .WithMessage("Principal name cannot exceed 100 characters");

        RuleFor(x => x.ContactNumber)
            .Matches(@"^\d{10}$")
            .WithMessage("Contact number must be 10 digits")
            .When(x => !string.IsNullOrEmpty(x.ContactNumber));

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.EstablishedDate)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Established date cannot be in the future");
    }
}