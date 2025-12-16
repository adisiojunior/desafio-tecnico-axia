using DesafioTecnicoAxia.Domain.Enumeradores;

namespace DesafioTecnicoAxia.Application.DTOs;

public class ListarVeiculosRequestDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Marca? Marca { get; set; }
    public string? Modelo { get; set; }
    public decimal? ValorMin { get; set; }
    public decimal? ValorMax { get; set; }
    public string? OrderBy { get; set; } = "CreatedAt";
    public string? SortOrder { get; set; } = "desc";
}

