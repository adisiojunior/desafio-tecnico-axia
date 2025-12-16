using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DesafioTecnicoAxia.Application.DTOs;
using DesafioTecnicoAxia.Domain.Enumeradores;
using DesafioTecnicoAxia.Infra.Context;

namespace DesafioTecnicoAxia.Tests.Integration;

public class VeiculoControllerIntegrationTests : IClassFixture<WebApplicationFactory<DesafioTecnicoAxia.WebApi.Program>>
{
    private readonly WebApplicationFactory<DesafioTecnicoAxia.WebApi.Program> _factory;

    public VeiculoControllerIntegrationTests(WebApplicationFactory<DesafioTecnicoAxia.WebApi.Program> factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClient(string? databaseName = null)
    {
        databaseName ??= $"TestDb_{Guid.NewGuid()}";
        
        return _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName);
                });
            });
        }).CreateClient();
    }

    [Fact]
    public async Task POST_CriarVeiculo_DeveRetornar201Created()
    {
        // Arrange
        var client = CreateClient();
        var dto = new CriarVeiculoDto
        {
            Descricao = "Carro esportivo de teste",
            Marca = Marca.Ford,
            Modelo = "Mustang GT",
            Valor = 250000.00m
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/veiculo", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var veiculo = await response.Content.ReadFromJsonAsync<VeiculoDto>();
        veiculo.Should().NotBeNull();
        veiculo!.Descricao.Should().Be(dto.Descricao);
        veiculo.Marca.Should().Be(dto.Marca);
        veiculo.Modelo.Should().Be(dto.Modelo);
        veiculo.Valor.Should().Be(dto.Valor);
        veiculo.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GET_ListarVeiculos_ComPaginação_DeveRetornar200OK()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        var client = CreateClient(dbName);
        // Criar alguns veículos primeiro
        for (int i = 0; i < 5; i++)
        {
            var dto = new CriarVeiculoDto
            {
                Descricao = $"Veículo teste {i}",
                Marca = Marca.Ford,
                Modelo = $"Modelo {i}",
                Valor = 100000 + (i * 10000)
            };
            await client.PostAsJsonAsync("/api/veiculo", dto);
        }

        // Act
        var response = await client.GetAsync("/api/veiculo?page=1&pageSize=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var resultado = await response.Content.ReadFromJsonAsync<PagedVeiculoResultDto>();
        resultado.Should().NotBeNull();
        resultado!.Data.Should().NotBeEmpty();
        resultado.Page.Should().Be(1);
        resultado.PageSize.Should().Be(10);
        resultado.TotalCount.Should().BeGreaterThanOrEqualTo(5);
    }

    [Fact]
    public async Task GET_ObterVeiculoPorId_DeveRetornar200OK()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        var client = CreateClient(dbName);
        var criarDto = new CriarVeiculoDto
        {
            Descricao = "Veículo para buscar",
            Marca = Marca.Chevrolet,
            Modelo = "Camaro",
            Valor = 300000.00m
        };

        var criarResponse = await client.PostAsJsonAsync("/api/veiculo", criarDto);
        var veiculoCriado = await criarResponse.Content.ReadFromJsonAsync<VeiculoDto>();

        // Act
        var response = await client.GetAsync($"/api/veiculo/{veiculoCriado!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var veiculo = await response.Content.ReadFromJsonAsync<VeiculoDto>();
        veiculo.Should().NotBeNull();
        veiculo!.Id.Should().Be(veiculoCriado.Id);
        veiculo.Modelo.Should().Be("Camaro");
    }

    [Fact]
    public async Task DELETE_ExcluirVeiculo_DeveRetornar204NoContent()
    {
        // Arrange
        var dbName = $"TestDb_{Guid.NewGuid()}";
        var client = CreateClient(dbName);
        var criarDto = new CriarVeiculoDto
        {
            Descricao = "Veículo para excluir",
            Marca = Marca.Fiat,
            Modelo = "Uno",
            Valor = 50000.00m
        };

        var criarResponse = await client.PostAsJsonAsync("/api/veiculo", criarDto);
        var veiculoCriado = await criarResponse.Content.ReadFromJsonAsync<VeiculoDto>();

        // Act
        var response = await client.DeleteAsync($"/api/veiculo/{veiculoCriado!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verificar que o veículo não é mais retornado (soft delete)
        var getResponse = await client.GetAsync($"/api/veiculo/{veiculoCriado.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

