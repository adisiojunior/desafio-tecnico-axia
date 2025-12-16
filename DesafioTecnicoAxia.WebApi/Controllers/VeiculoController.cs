using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MediatR;
using DesafioTecnicoAxia.Application.Commands;
using DesafioTecnicoAxia.Application.DTOs;

namespace DesafioTecnicoAxia.WebApi.Controllers;

/// <summary>
/// Controller para gerenciamento de veículos
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VeiculoController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public VeiculoController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Cadastra um novo veículo
    /// </summary>
    /// <param name="dto">Dados do veículo a ser cadastrado</param>
    /// <returns>Veículo criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(VeiculoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VeiculoDto>> Adicionar([FromBody] CriarVeiculoDto dto)
    {
        var command = _mapper.Map<AdicionarVeiculoCommand>(dto);
        var veiculo = await _mediator.Send(command);
        var veiculoDto = _mapper.Map<VeiculoDto>(veiculo);
        
        return CreatedAtAction(nameof(ObterPorId), new { id = veiculoDto.Id }, veiculoDto);
    }

    /// <summary>
    /// Atualiza um veículo existente
    /// </summary>
    /// <param name="id">Id do veículo</param>
    /// <param name="dto">Dados atualizados do veículo</param>
    /// <returns>Veículo atualizado</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(VeiculoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VeiculoDto>> Atualizar(Guid id, [FromBody] AtualizarVeiculoDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("O Id da URL não corresponde ao Id do DTO.");
        }

        var command = _mapper.Map<AtualizarVeiculoCommand>(dto);
        var veiculo = await _mediator.Send(command);
        var veiculoDto = _mapper.Map<VeiculoDto>(veiculo);
        
        return Ok(veiculoDto);
    }

    /// <summary>
    /// Obtém um veículo por Id
    /// </summary>
    /// <param name="id">Id do veículo</param>
    /// <returns>Veículo encontrado</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VeiculoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VeiculoDto>> ObterPorId(Guid id)
    {
        var query = new ObterVeiculoPorIdQuery { Id = id };
        var veiculo = await _mediator.Send(query);

        if (veiculo == null)
        {
            return NotFound($"Veículo com Id {id} não encontrado.");
        }

        var veiculoDto = _mapper.Map<VeiculoDto>(veiculo);
        return Ok(veiculoDto);
    }

    /// <summary>
    /// Lista veículos com paginação e filtros
    /// </summary>
    /// <param name="request">Parâmetros de paginação e filtros</param>
    /// <returns>Lista paginada de veículos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedVeiculoResultDto), StatusCodes.Status200OK)]
    [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "page", "pageSize", "marca", "modelo", "valorMin", "valorMax", "orderBy", "sortOrder" })]
    public async Task<ActionResult<PagedVeiculoResultDto>> Listar([FromQuery] ListarVeiculosRequestDto request)
    {
        var query = new ListarVeiculosQuery
        {
            Page = request.Page,
            PageSize = request.PageSize,
            Marca = request.Marca,
            Modelo = request.Modelo,
            ValorMin = request.ValorMin,
            ValorMax = request.ValorMax,
            OrderBy = request.OrderBy,
            SortOrder = request.SortOrder
        };
        
        var resultado = await _mediator.Send(query);
        var veiculosDto = _mapper.Map<IEnumerable<VeiculoDto>>(resultado.Data);
        
        var pagedResult = new PagedVeiculoResultDto
        {
            Data = veiculosDto,
            Page = resultado.Page,
            PageSize = resultado.PageSize,
            TotalCount = resultado.TotalCount,
            TotalPages = resultado.TotalPages,
            HasPrevious = resultado.HasPrevious,
            HasNext = resultado.HasNext
        };
        
        return Ok(pagedResult);
    }

    /// <summary>
    /// Remove um veículo
    /// </summary>
    /// <param name="id">Id do veículo a ser removido</param>
    /// <returns>No content se removido com sucesso</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var command = new ExcluirVeiculoCommand { Id = id };
        await _mediator.Send(command);
        
        return NoContent();
    }
}
