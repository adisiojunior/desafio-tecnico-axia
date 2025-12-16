using MediatR;
using DesafioTecnicoAxia.Domain.Entidades;

namespace DesafioTecnicoAxia.Application.Commands;

public class ObterVeiculoPorIdQuery : IRequest<Veiculo?>
{
    public Guid Id { get; set; }
}

