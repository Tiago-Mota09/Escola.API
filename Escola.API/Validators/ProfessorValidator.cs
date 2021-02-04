using Escola.API.Domain.Models.Request;
using FluentValidation;

namespace Escola.API.Validators
{
    public class ProfessorValidator : AbstractValidator<ProfessorRequest>
    {
        public ProfessorValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Informe o nome")
                .MinimumLength(5).WithMessage("O nome deve ter no mínimo 5 caracteres")
                .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres")

                .DependentRules(() =>
                {
                    RuleFor(x => x.Idade)
                       .GreaterThan(18).WithMessage("A idade deve ser maior que 18 anos.")
                       .LessThanOrEqualTo(80).WithMessage("A idade do professor deve ser menor ou igual a 80 anos.")
                       .DependentRules(() =>
                       {
                           RuleFor(x => x.IdUnidade)
                               .GreaterThan(0).WithMessage("Informe a unidade.")
                               .NotNull().WithMessage("Este valor não pode ser nulo");
                       });
                });
        }
    }
}
