using Escola.API.Domain.Models.Request;
using FluentValidation;

namespace Escola.API.Validators
{
    public class ProfessorAlunoUpdateValidator : AbstractValidator<ProfessorAlunoUpdateRequest>
    {
        public ProfessorAlunoUpdateValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.IdProfessor)
               .NotNull().WithMessage("Este campo não pode ser nulo, informe o professor")
               .GreaterThan(0).WithMessage("Este campo deve ser maior que 0, informe o professor")

               .DependentRules(() =>
               {
                 RuleFor(x => x.IdAluno)
                   .NotNull().WithMessage("Este campo não pode ser nulo, informe o aluno")
                   .GreaterThan(0).WithMessage("Este campo deve ser maior que 0, informe o aluno")

                   .DependentRules(() =>
                   {
                       RuleFor(x => x.IdProfessorAluno)
                        .NotNull().WithMessage("Este campo não pode ser nulo, informe o campo professrorAluno corretamente")
                        .GreaterThan(0).WithMessage("Este campo deve ser maior que 0, informe o campo professrorAluno corretamente");
                   });
            });
        }
    }
}
