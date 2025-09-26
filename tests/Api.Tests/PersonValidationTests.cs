using FluentValidation.TestHelper;
using VaccinationCard.Application.Persons.Commands;
using VaccinationCard.Application.Persons.Validation;
using Xunit;

public class PersonValidationTests
{
    [Fact]
    public void CreatePerson_Invalid_ShouldHaveErrors()
    {
        var v = new CreatePersonValidator();
        var result = v.TestValidate(new CreatePersonCommand("", ""));
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.DocumentId);
    }

    [Fact]
    public void CreatePerson_Valid_ShouldPass()
    {
        var v = new CreatePersonValidator();
        var result = v.TestValidate(new CreatePersonCommand("Ana", "ID-123"));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
