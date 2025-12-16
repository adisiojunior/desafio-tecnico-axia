using Moq;
using AutoMapper;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using DesafioTecnicoAxia.Application.Commands;
using DesafioTecnicoAxia.Application.Handlers;
using DesafioTecnicoAxia.Application.VeiculoService;
using DesafioTecnicoAxia.Application.Mappings;
using DesafioTecnicoAxia.Domain.Entidades;
using DesafioTecnicoAxia.Domain.Enumeradores;

namespace DesafioTecnicoAxia.Tests.Handlers;

public class AdicionarVeiculoHandlerTests
{
    private readonly Mock<IVeiculoService> _veiculoServiceMock;
    private readonly Mock<ILogger<AdicionarVeiculoHandler>> _loggerMock;
    private readonly IMapper _mapper;
    private readonly AdicionarVeiculoHandler _handler;

    public AdicionarVeiculoHandlerTests()
    {
        _veiculoServiceMock = new Mock<IVeiculoService>();
        _loggerMock = new Mock<ILogger<AdicionarVeiculoHandler>>();

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = mapperConfig.CreateMapper();

        _handler = new AdicionarVeiculoHandler(
            _veiculoServiceMock.Object,
            _mapper,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_DeveAdicionarVeiculoComSucesso()
    {
        // Arrange
        var command = new AdicionarVeiculoCommand
        {
            Descricao = "Carro esportivo",
            Marca = Marca.Ford,
            Modelo = "Mustang",
            Valor = 250000.00m
        };

        var veiculoEsperado = Veiculo.Create(
            command.Descricao,
            command.Marca,
            command.Modelo,
            null,
            command.Valor);

        _veiculoServiceMock
            .Setup(x => x.AdicionarAsync(It.IsAny<Veiculo>()))
            .ReturnsAsync(veiculoEsperado);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Descricao.Should().Be(command.Descricao);
        resultado.Marca.Should().Be(command.Marca);
        resultado.Modelo.Should().Be(command.Modelo);
        resultado.Valor.Should().Be(command.Valor);
        
        _veiculoServiceMock.Verify(x => x.AdicionarAsync(It.IsAny<Veiculo>()), Times.Once);
    }
}

