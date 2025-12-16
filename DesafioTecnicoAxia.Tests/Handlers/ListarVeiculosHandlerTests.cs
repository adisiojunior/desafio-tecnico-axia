using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using DesafioTecnicoAxia.Application.Commands;
using DesafioTecnicoAxia.Domain.Common;
using DesafioTecnicoAxia.Application.Handlers;
using DesafioTecnicoAxia.Application.VeiculoService;
using DesafioTecnicoAxia.Domain.Entidades;
using DesafioTecnicoAxia.Domain.Enumeradores;

namespace DesafioTecnicoAxia.Tests.Handlers;

public class ListarVeiculosHandlerTests
{
    private readonly Mock<IVeiculoService> _veiculoServiceMock;
    private readonly Mock<ILogger<ListarVeiculosHandler>> _loggerMock;
    private readonly ListarVeiculosHandler _handler;

    public ListarVeiculosHandlerTests()
    {
        _veiculoServiceMock = new Mock<IVeiculoService>();
        _loggerMock = new Mock<ILogger<ListarVeiculosHandler>>();
        _handler = new ListarVeiculosHandler(_veiculoServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarResultadoPaginado()
    {
        // Arrange
        var veiculos = new List<Veiculo>
        {
            Veiculo.Create("Carro 1", Marca.Ford, "Modelo 1", null, 100000),
            Veiculo.Create("Carro 2", Marca.Chevrolet, "Modelo 2", null, 200000)
        };

        var pagedResult = new PagedResult<Veiculo>
        {
            Data = veiculos,
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };

        var query = new ListarVeiculosQuery
        {
            Page = 1,
            PageSize = 10
        };

        _veiculoServiceMock
            .Setup(x => x.ListarComFiltrosAsync(1, 10, null, null, null, null, "CreatedAt", "desc"))
            .ReturnsAsync(pagedResult);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Data.Should().HaveCount(2);
        resultado.Page.Should().Be(1);
        resultado.PageSize.Should().Be(10);
        resultado.TotalCount.Should().Be(2);
        resultado.TotalPages.Should().Be(1);
        
        _veiculoServiceMock.Verify(x => x.ListarComFiltrosAsync(1, 10, null, null, null, null, "CreatedAt", "desc"), Times.Once);
    }

    [Fact]
    public async Task Handle_ComFiltros_DeveAplicarFiltros()
    {
        // Arrange
        var query = new ListarVeiculosQuery
        {
            Page = 1,
            PageSize = 10,
            Marca = Marca.Ford,
            Modelo = "Mustang",
            ValorMin = 100000,
            ValorMax = 300000
        };

        var pagedResult = new PagedResult<Veiculo>
        {
            Data = new List<Veiculo>(),
            Page = 1,
            PageSize = 10,
            TotalCount = 0
        };

        _veiculoServiceMock
            .Setup(x => x.ListarComFiltrosAsync(1, 10, Marca.Ford, "Mustang", 100000, 300000, "CreatedAt", "desc"))
            .ReturnsAsync(pagedResult);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        _veiculoServiceMock.Verify(x => x.ListarComFiltrosAsync(
            1, 10, Marca.Ford, "Mustang", 100000, 300000, "CreatedAt", "desc"), Times.Once);
    }
}

