using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class CreateNoticeValidator : AbstractValidator<CreateNoticeDto>
{
    public CreateNoticeValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200)
            .WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message is required")
            .MaximumLength(2000)
            .WithMessage("Message cannot exceed 2000 characters");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required")
            .Must(x => new[] { "Information", "Request", "Announcement", "General", "Urgent" }.Contains(x))
            .WithMessage("Category must be Information, Request, Announcement, General, or Urgent");

        RuleFor(x => x.Priority)
            .NotEmpty()
            .WithMessage("Priority is required")
            .Must(x => new[] { "Low", "Medium", "High", "Urgent" }.Contains(x))
            .WithMessage("Priority must be Low, Medium, High, or Urgent");
    }
}