using FluentValidation;
using VaccinationCard.Application.Persons.Commands;

namespace VaccinationCard.Application.Persons.Validation;

public class CreatePersonValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DocumentId).NotEmpty().MaximumLength(50);
        // se quiser CPF: Regex(@"^\d{11}$") (mas deixe genérico por agora)
    }
}
