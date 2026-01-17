using Application.DTOs;
using Domain.Entities;
using FluentValidation;

namespace Application.Validators;

public class CreatePollValidator : AbstractValidator<CreatePollDto>
{
    public CreatePollValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.EndDate)
            .GreaterThan(DateTime.UtcNow).When(x => x.EndDate.HasValue)
            .WithMessage("End date must be in the future");

        RuleFor(x => x.Questions)
            .NotEmpty().WithMessage("At least one question is required")
            .Must(q => q.Count > 0).WithMessage("At least one question is required");

        RuleForEach(x => x.Questions).SetValidator(new CreatePollQuestionValidator());
    }
}

public class CreatePollQuestionValidator : AbstractValidator<CreatePollQuestionDto>
{
    public CreatePollQuestionValidator()
    {
        RuleFor(x => x.QuestionText)
            .NotEmpty().WithMessage("Question text is required")
            .MaximumLength(500).WithMessage("Question text cannot exceed 500 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid question type");

        When(x => x.Type == QuestionType.MultipleChoice || x.Type == QuestionType.Checkbox, () =>
        {
            RuleFor(x => x.Options)
                .NotEmpty().WithMessage("Options are required for multiple choice and checkbox questions")
                .Must(opts => opts != null && opts.Count >= 2).WithMessage("At least 2 options are required for multiple choice and checkbox questions");

            RuleForEach(x => x.Options).SetValidator(new CreatePollOptionValidator());
        });

        When(x => x.Type == QuestionType.Rating, () =>
        {
            RuleFor(x => x.Options)
                .Empty().WithMessage("Rating questions should not have predefined options");
        });

        When(x => x.Type == QuestionType.Text, () =>
        {
            RuleFor(x => x.Options)
                .Empty().WithMessage("Text questions should not have predefined options");
        });
    }
}

public class CreatePollOptionValidator : AbstractValidator<CreatePollOptionDto>
{
    public CreatePollOptionValidator()
    {
        RuleFor(x => x.OptionText)
            .NotEmpty().WithMessage("Option text is required")
            .MaximumLength(200).WithMessage("Option text cannot exceed 200 characters");
    }
}

public class UpdatePollValidator : AbstractValidator<UpdatePollDto>
{
    public UpdatePollValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Valid poll ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.EndDate)
            .GreaterThan(DateTime.UtcNow).When(x => x.EndDate.HasValue)
            .WithMessage("End date must be in the future");
    }
}

public class SubmitPollResponseValidator : AbstractValidator<SubmitPollResponseDto>
{
    public SubmitPollResponseValidator()
    {
        RuleFor(x => x.PollId)
            .GreaterThan(0).WithMessage("Valid poll ID is required");

        RuleFor(x => x.Answers)
            .NotEmpty().WithMessage("At least one answer is required");

        RuleForEach(x => x.Answers).SetValidator(new SubmitPollAnswerValidator());
    }
}

public class SubmitPollAnswerValidator : AbstractValidator<SubmitPollAnswerDto>
{
    public SubmitPollAnswerValidator()
    {
        RuleFor(x => x.PollQuestionId)
            .GreaterThan(0).WithMessage("Valid question ID is required");

        When(x => x.PollOptionId.HasValue, () =>
        {
            RuleFor(x => x.PollOptionId.Value)
                .GreaterThan(0).WithMessage("Valid option ID is required");

            RuleFor(x => x.TextAnswer)
                .Empty().WithMessage("Text answer should not be provided when option is selected");

            RuleFor(x => x.RatingValue)
                .Null().WithMessage("Rating should not be provided when option is selected");
        });

        When(x => !string.IsNullOrEmpty(x.TextAnswer), () =>
        {
            RuleFor(x => x.TextAnswer)
                .MaximumLength(1000).WithMessage("Text answer cannot exceed 1000 characters");

            RuleFor(x => x.PollOptionId)
                .Null().WithMessage("Option should not be provided when text answer is given");

            RuleFor(x => x.RatingValue)
                .Null().WithMessage("Rating should not be provided when text answer is given");
        });

        When(x => x.RatingValue.HasValue, () =>
        {
            RuleFor(x => x.RatingValue.Value)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.PollOptionId)
                .Null().WithMessage("Option should not be provided when rating is given");

            RuleFor(x => x.TextAnswer)
                .Empty().WithMessage("Text answer should not be provided when rating is given");
        });

        When(x => !x.PollOptionId.HasValue && string.IsNullOrEmpty(x.TextAnswer) && !x.RatingValue.HasValue, () =>
        {
            RuleFor(x => x).Must(x => false).WithMessage("Either option, text answer, or rating must be provided");
        });
    }
}
