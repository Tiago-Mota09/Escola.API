using Escola.API.Domain.Models.Request;
using FluentValidation;

namespace Escola.API.Validators
{
    public class AlunoUpdateValidator : AbstractValidator<AlunoUpdateRequest>
    {
        public AlunoUpdateValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Informe o nome")
                .MinimumLength(10).WithMessage("O nome deve ter no mínimo 10 caracteres")
                .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Idade)
                       .GreaterThanOrEqualTo(6).WithMessage("A idade do aluno deve ser maior ou igual a 6 anos.")
                       .LessThanOrEqualTo(20).WithMessage("A idade do aluno deve ser menor ou igual a 20 anos.")
                       .DependentRules(() =>
                       {
                           RuleFor(x => x.IdUnidade)
                               .GreaterThan(0).WithMessage("Informe a unidade.");
                            //.LessThanOrEqualTo(100).WithMessage("A unidade informada não existe.");
                        });
                });
        }
    }
}
