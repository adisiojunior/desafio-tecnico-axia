using FluentValidation;
using DesafioTecnicoAxia.Application.Commands;

namespace DesafioTecnicoAxia.Application.Validators;

public class ExcluirVeiculoValidator : AbstractValidator<ExcluirVeiculoCommand>
{
    public ExcluirVeiculoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O Id é obrigatório.");
    }
}

