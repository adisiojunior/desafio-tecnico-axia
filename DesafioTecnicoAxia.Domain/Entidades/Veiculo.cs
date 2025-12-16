using DesafioTecnicoAxia.Domain.Enumeradores;

namespace DesafioTecnicoAxia.Domain.Entidades;

public class Veiculo : BaseEntity
{
    public string Descricao { get; internal set; } = string.Empty;
    public Marca Marca { get; internal set; }
    public string Modelo { get; internal set; } = string.Empty;
    public string? Opcionais { get; internal set; }
    public decimal? Valor { get; internal set; }

    protected Veiculo() { }

    public static Veiculo Create(string descricao, Marca marca, string modelo, string? opcionais = null, decimal? valor = null)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição não pode ser vazia.", nameof(descricao));
        
        if (string.IsNullOrWhiteSpace(modelo))
            throw new ArgumentException("Modelo não pode ser vazio.", nameof(modelo));

        return new Veiculo
        {
            Descricao = descricao,
            Marca = marca,
            Modelo = modelo,
            Opcionais = opcionais,
            Valor = valor
        };
    }

    public void Update(string descricao, Marca marca, string modelo, string? opcionais = null, decimal? valor = null)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição não pode ser vazia.", nameof(descricao));
        
        if (string.IsNullOrWhiteSpace(modelo))
            throw new ArgumentException("Modelo não pode ser vazio.", nameof(modelo));

        Descricao = descricao;
        Marca = marca;
        Modelo = modelo;
        Opcionais = opcionais;
        Valor = valor;
        MarkAsUpdated();
    }
}

