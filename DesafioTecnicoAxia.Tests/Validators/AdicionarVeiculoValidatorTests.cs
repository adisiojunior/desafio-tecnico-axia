using Xunit;
using FluentAssertions;
using DesafioTecnicoAxia.Application.Commands;
using DesafioTecnicoAxia.Application.Validators;
using DesafioTecnicoAxia.Domain.Enumeradores;

namespace DesafioTecnicoAxia.Tests.Validators;

public class AdicionarVeiculoValidatorTests
{
    private readonly AdicionarVeiculoValidator _validator;

    public AdicionarVeiculoValidatorTests()
    {
        _validator = new AdicionarVeiculoValidator();
    }

    [Fact]
    public void Validate_QuandoDescricaoVazia_DeveRetornarErro()
    {
        // Arrange
        var command = new AdicionarVeiculoCommand
        {
            Descricao = string.Empty,
            Marca = Marca.Ford,
            Modelo = "Mustang"
        };

        // Act
        var resultado = _validator.Validate(command);

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == "Descricao");
    }

    [Fact]
    public void Validate_QuandoModeloVazia_DeveRetornarErro()
    {
        // Arrange
        var command = new AdicionarVeiculoCommand
        {
            Descricao = "Carro esportivo",
            Marca = Marca.Ford,
            Modelo = string.Empty
        };

        // Act
        var resultado = _validator.Validate(command);

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == "Modelo");
    }

    [Fact]
    public void Validate_QuandoValorMenorOuIgualZero_DeveRetornarErro()
    {
        // Arrange
        var command = new AdicionarVeiculoCommand
        {
            Descricao = "Carro esportivo",
            Marca = Marca.Ford,
            Modelo = "Mustang",
            Valor = -100
        };

        // Act
        var resultado = _validator.Validate(command);

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == "Valor");
    }

    [Fact]
    public void Validate_QuandoDadosValidos_DeveSerValido()
    {
        // Arrange
        var command = new AdicionarVeiculoCommand
        {
            Descricao = "Carro esportivo",
            Marca = Marca.Ford,
            Modelo = "Mustang",
            Valor = 250000.00m
        };

        // Act
        var resultado = _validator.Validate(command);

        // Assert
        resultado.IsValid.Should().BeTrue();
    }
}

