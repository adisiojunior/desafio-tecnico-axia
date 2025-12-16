namespace DesafioTecnicoAxia.Application.DTOs;

public class PagedVeiculoResultDto
{
    public IEnumerable<VeiculoDto> Data { get; set; } = Enumerable.Empty<VeiculoDto>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
}

