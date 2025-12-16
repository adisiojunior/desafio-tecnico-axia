using FluentValidation;
using DesafioTecnicoAxia.Application.Commands;

namespace DesafioTecnicoAxia.Application.Validators;

public class ObterVeiculoPorIdValidator : AbstractValidator<ObterVeiculoPorIdQuery>
{
    public ObterVeiculoPorIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O Id é obrigatório.");
    }
}

