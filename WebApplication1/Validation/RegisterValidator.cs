using FluentValidation;
using WebApplication1.Models;

namespace WebApplication1.Validation;

public class RegisterValidator : AbstractValidator<Users>
{
    public RegisterValidator()
    {
        RuleFor(users => users.login).NotEmpty().MaximumLength(30);
        RuleFor(users => users.first_name).NotEmpty().MaximumLength(50);
        RuleFor(users => users.last_name).NotEmpty().MaximumLength(50);
        RuleFor(users => users.password).NotEmpty().MaximumLength(50).Matches(@"^(?=.*\d)(?=.*[A-Z])(?=.*[-+*/]).*$");
        RuleFor(users => users.email).NotEmpty().MaximumLength(30).EmailAddress();
    }
}