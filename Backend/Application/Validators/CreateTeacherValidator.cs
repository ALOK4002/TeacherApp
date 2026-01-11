using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class CreateTeacherValidator : AbstractValidator<CreateTeacherDto>
{
    public CreateTeacherValidator()
    {
        RuleFor(x => x.TeacherName)
            .NotEmpty()
            .WithMessage("Teacher name is required")
            .MaximumLength(100)
            .WithMessage("Teacher name cannot exceed 100 characters");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.District)
            .NotEmpty()
            .WithMessage("District is required")
            .MaximumLength(100)
            .WithMessage("District cannot exceed 100 characters");

        RuleFor(x => x.Pincode)
            .NotEmpty()
            .WithMessage("Pincode is required")
            .Matches(@"^\d{6}$")
            .WithMessage("Pincode must be 6 digits");

        RuleFor(x => x.SchoolId)
            .GreaterThan(0)
            .WithMessage("Valid school selection is required");

        RuleFor(x => x.ClassTeaching)
            .NotEmpty()
            .WithMessage("Class teaching is required")
            .MaximumLength(50)
            .WithMessage("Class teaching cannot exceed 50 characters");

        RuleFor(x => x.Subject)
            .NotEmpty()
            .WithMessage("Subject is required")
            .MaximumLength(100)
            .WithMessage("Subject cannot exceed 100 characters");

        RuleFor(x => x.Qualification)
            .NotEmpty()
            .WithMessage("Qualification is required")
            .MaximumLength(200)
            .WithMessage("Qualification cannot exceed 200 characters");

        RuleFor(x => x.ContactNumber)
            .NotEmpty()
            .WithMessage("Contact number is required")
            .Matches(@"^\d{10}$")
            .WithMessage("Contact number must be 10 digits");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.DateOfJoining)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Date of joining cannot be in the future");

        RuleFor(x => x.Salary)
            .GreaterThan(0)
            .WithMessage("Salary must be greater than 0");
    }
}