using DesafioTecnicoAxia.Domain.Enumeradores;

namespace DesafioTecnicoAxia.Application.DTOs;

public class VeiculoDto
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public Marca Marca { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public string? Opcionais { get; set; }
    public decimal? Valor { get; set; }
}

