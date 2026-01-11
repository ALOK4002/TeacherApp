using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class CreateNoticeReplyValidator : AbstractValidator<CreateNoticeReplyDto>
{
    public CreateNoticeReplyValidator()
    {
        RuleFor(x => x.NoticeId)
            .GreaterThan(0)
            .WithMessage("Valid notice ID is required");

        RuleFor(x => x.ReplyMessage)
            .NotEmpty()
            .WithMessage("Reply message is required")
            .MaximumLength(1000)
            .WithMessage("Reply message cannot exceed 1000 characters");
    }
}