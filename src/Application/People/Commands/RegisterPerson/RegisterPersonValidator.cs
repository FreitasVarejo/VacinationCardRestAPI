using FluentValidation;

namespace Application.People.Commands.RegisterPerson
{
    public class RegisterPersonValidator : AbstractValidator<RegisterPersonCommand>
    {
        public RegisterPersonValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
            RuleFor(x => x.DocNumber).NotEmpty().MaximumLength(50);
        }
    }
}
