using AutoMapper;
using DesafioTecnicoAxia.Application.DTOs;
using DesafioTecnicoAxia.Application.Commands;
using DesafioTecnicoAxia.Domain.Entidades;

namespace DesafioTecnicoAxia.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Veiculo, VeiculoDto>().ReverseMap();
        CreateMap<CriarVeiculoDto, AdicionarVeiculoCommand>();
        CreateMap<AtualizarVeiculoDto, AtualizarVeiculoCommand>();
        CreateMap<AtualizarVeiculoCommand, Veiculo>();
    }
}

