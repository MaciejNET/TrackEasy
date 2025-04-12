using FluentValidation;

namespace TrrackEasy.Domain.Stations;

public class CityValidator : AbstractValidator<city>
{
    public CityValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty().Length(3, 50);
        RuleFor(c => c.CityEnum).IsInEnum();
    }
}