
namespace FinancialControl.Domain.Entities;

public class Cartao
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public decimal LimiteTotal { get; private set; }
    public decimal LimiteDisponivel { get; private set; }
    public int DiaFechamentoFatura { get; private set; }
    public int DiaVencimentoFatura { get; private set; }
    
    public int ContaDePagamentoId { get; private set; }
    public ContaBancaria ContaDePagamento { get; private set; } = null!;
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;

    public Cartao() { }
    
    public Cartao(string nome, decimal limiteTotal, int diaFechamentoFatura, int diaVencimentoFatura)
    {
        ValidarNome(nome);
        
        Nome = nome;
        LimiteTotal = limiteTotal;
        LimiteDisponivel = limiteTotal;
        DiaFechamentoFatura = diaFechamentoFatura;
        DiaVencimentoFatura = diaVencimentoFatura;
    }
    
    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do cartão não pode ser vazio ou nulo.");
        if (nome.Length < 3)
            throw new ArgumentException("O nome do cartão deve ter pelo menos 3 caracteres.");
    }

    public void AdicionarUsuarioId(int usuarioId)
    {
        UsuarioId = usuarioId;
    }
    
    public void AtualizarContaDePagamentoId(int contaDePagamentoId)
    {
        ContaDePagamentoId = contaDePagamentoId;
    }
    
    public void AtualizarCartao(string nome, decimal limiteTotal, int diaFechamentoFatura, int diaVencimentoFatura, int contaDePagamentoId)
    {
        ValidarNome(nome);
        
        Nome = nome;
        LimiteTotal = limiteTotal;
        LimiteDisponivel = limiteTotal;
        DiaFechamentoFatura = diaFechamentoFatura;
        DiaVencimentoFatura = diaVencimentoFatura;
        ContaDePagamentoId = contaDePagamentoId;
    }
    
}