using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.UserNameOrEmail)
            .NotEmpty()
            .WithMessage("Username or Email is required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}