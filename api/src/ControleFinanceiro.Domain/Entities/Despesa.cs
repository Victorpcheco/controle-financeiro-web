namespace ControleFinanceiro.Domain.Entities;

public class Despesa
{
    public int Id { get; private set; }
    public string TituloDespesa { get; private set; } = null!;
    public string Data { get; private set; } = null!;
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

    public Despesa(string tituloDespesa, string data, int mesReferenciaId, int categoriaId, int cartaoId, int contaBancariaId, decimal valor, bool realizado, int usuarioId)
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
    
    private static void ValidarData(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
            throw new ArgumentException("Data não pode ser vazia ou nula.");

        if (!DateTime.TryParseExact(data, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _))
            throw new ArgumentException("Data deve estar no formato DD/MM/AAAA.");
    }
    
    public void AtualizarDespesa(string tituloDespesa, string data, int mesReferenciaId, int categoriaId, int cartaoId, int contaBancariaId, decimal valor, bool realizado)
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