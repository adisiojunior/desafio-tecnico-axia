using FluentValidation;
using DesafioTecnicoAxia.Application.Commands;

namespace DesafioTecnicoAxia.Application.Validators;

public class AdicionarVeiculoValidator : AbstractValidator<AdicionarVeiculoCommand>
{
    public AdicionarVeiculoValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.");

        RuleFor(x => x.Marca)
            .IsInEnum().WithMessage("A marca deve ser um valor válido do enumerador.");

        RuleFor(x => x.Modelo)
            .NotEmpty().WithMessage("O modelo é obrigatório.")
            .MaximumLength(200).WithMessage("O modelo deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Opcionais)
            .MaximumLength(1000).WithMessage("Os opcionais devem ter no máximo 1000 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Opcionais));

        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("O valor deve ser maior que zero.")
            .When(x => x.Valor.HasValue);
    }
}

