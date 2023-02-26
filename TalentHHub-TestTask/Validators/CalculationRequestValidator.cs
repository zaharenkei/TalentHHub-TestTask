using FluentValidation;
using TalentHHub_TestTask.Handlers.Models;

namespace TalentHHub_TestTask.Validators
{
    public class CalculationRequestValidator : AbstractValidator<CalculationRequest>
    {
        public CalculationRequestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Items).NotEmpty();
            RuleForEach(x => x.Items)
                .ChildRules(i =>
                {
                    i.RuleFor(x => x.Name).NotEmpty();
                    i.RuleFor(x => x.Cost).GreaterThan(0);
                });
        }
    }
}
