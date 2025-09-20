using FluentValidation;

namespace Application.Vaccines.Commands.RegisterVaccine
{
    public class RegisterVaccineValidator : AbstractValidator<RegisterVaccineCommand>
    {
        public RegisterVaccineValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.TotalDoses).GreaterThanOrEqualTo(1).LessThanOrEqualTo(10);
        }
    }
}
