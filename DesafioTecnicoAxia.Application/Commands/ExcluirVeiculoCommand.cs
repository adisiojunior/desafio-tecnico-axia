using MediatR;

namespace DesafioTecnicoAxia.Application.Commands;

public class ExcluirVeiculoCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

