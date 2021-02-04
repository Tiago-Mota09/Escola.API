using Escola.API.Domain.Models.Request;
using FluentValidation;

namespace Escola.API.Validators
{
    public class UnidadeValidator : AbstractValidator<UnidadeRequest>
    {
        public UnidadeValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Nome)
               .NotEmpty().WithMessage("Informe o nome")
               .MinimumLength(5).WithMessage("O nome deve ter no mínimo 5 caracteres")
               .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres")
               .DependentRules(() =>
                {
                    RuleFor(x => x.Endereco)
                      .MinimumLength(6).WithMessage("O endereco deve ter no mínimo 6 caracteres.")
                      .MaximumLength(150).WithMessage("O endereço deve ter no máximo 150 caracteres.");
                });
        }
    }
}

