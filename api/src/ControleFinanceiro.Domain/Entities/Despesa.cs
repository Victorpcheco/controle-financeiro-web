

namespace ControleFinanceiro.Domain.Entities;

public class Despesa
{
    public int Id { get; private set; }
    public string TituloDespesa { get; private set; } = null!;
    public DateOnly Data { get; private set; }
    public int MesReferenciaId { get; private set; }
    public MesReferencia MesReferencia { get; private set; } = null!;
    public int CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; } = null!;
    public int CartaoId { get; private set; }
    public Cartao Cartao { get; private set; } = null!;
    public int ContaBancariaId { get; private set; }
    public ContaBancaria ContaBancaria { get; private set; } = null!;
    public decimal Valor { get; private set; }
    public bool Realizado { get; private set; } = false;
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;

    public Despesa()
    {
        
    }

    public Despesa(string tituloDespesa, DateOnly data, int mesReferenciaId, int categoriaId, int cartaoId, int contaBancariaId, decimal valor, bool realizado, int usuarioId)
    {

        ValidarTituloDespesa(tituloDespesa);
        ValidarData(data);
        
        TituloDespesa = tituloDespesa;
        Data = data;
        MesReferenciaId = mesReferenciaId;
        CategoriaId = categoriaId;
        CartaoId = cartaoId;
        ContaBancariaId = contaBancariaId;
        Valor = valor;
        Realizado = realizado;
        UsuarioId = usuarioId;
    }
    
    private static void ValidarTituloDespesa(string tituloDespesa)
    {
        if (string.IsNullOrWhiteSpace(tituloDespesa))
            throw new ArgumentException("Título da despesa não pode ser vazio ou nulo.");
        
        if (tituloDespesa.Length > 100)
            throw new ArgumentException("Título da despesa não pode ter mais de 100 caracteres.");
    }
    
    private void ValidarData(DateOnly data)
    {
        if (data == default)
            throw new ArgumentException("Data não pode ser vazia.");
    
        if (data < DateOnly.FromDateTime(DateTime.Now.AddYears(-100)))
            throw new ArgumentException("Data muito antiga.");
    }
    
    public void AtualizarDespesa(string tituloDespesa, DateOnly data, int mesReferenciaId, int categoriaId, int cartaoId, int contaBancariaId, decimal valor, bool realizado)
    {
        ValidarTituloDespesa(tituloDespesa);
        ValidarData(data);
        
        TituloDespesa = tituloDespesa;
        Data = data;
        MesReferenciaId = mesReferenciaId;
        CategoriaId = categoriaId;
        CartaoId = cartaoId;
        ContaBancariaId = contaBancariaId;
        Valor = valor;
        Realizado = realizado;
    }
}