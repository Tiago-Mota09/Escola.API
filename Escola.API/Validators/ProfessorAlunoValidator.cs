using Escola.API.Domain.Models.Request;
using FluentValidation;

namespace Escola.API.Validators
{
    public class ProfessorAlunoValidator : AbstractValidator<ProfessorAlunoRequest>
    {
        public ProfessorAlunoValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.IdProfessor)
                .NotEmpty().WithMessage("Este campo não pode ser vazio, informe o professor")
                .NotNull().WithMessage("Este campo não pode ser nulo, informe o professor")
                .GreaterThan(0).WithMessage("Informe o professor")

                .DependentRules(() =>
                {
                    RuleFor(x => x.IdAluno)
                       .GreaterThan(0).WithMessage("Este campo não pode ser vazio, informe o aluno")
                       .NotNull().WithMessage("Este campo deve ser maior que 0, informe o aluno");
                });
        }
    }
}
