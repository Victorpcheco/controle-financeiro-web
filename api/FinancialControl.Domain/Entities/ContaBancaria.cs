using System.ComponentModel.DataAnnotations;

namespace FinancialControl.Domain.Entities;

public class ContaBancaria
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "O nome da conta é obrigatório.")]
    public string Nome { get; set; }
    [Required(ErrorMessage = "É necessário informar o banco.")]
    public string Banco { get; set; }
    public decimal SaldoInicial { get; private set; } 
    public decimal? SaldoAtual { get; private set; }
    public DateTime? DataCriacao { get; set; } = DateTime.Now;
    [Required]
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public ContaBancaria()
    {
        
    }

    public ContaBancaria(string nome, string banco, decimal saldoInicial, int usuarioId)
    {
        Nome = nome;
        Banco = banco;
        SaldoInicial = saldoInicial;
        SaldoAtual = saldoInicial;
        DataCriacao = DateTime.Now;
        UsuarioId = usuarioId;
    }
    public static ContaBancaria Criar(string nome, string banco, decimal saldoInicial, int usuarioId)
    {
        return new ContaBancaria(nome, banco, saldoInicial, usuarioId);
    }
    public void AtualizarContaBancaria(string nome, string banco, decimal saldoAtual)
    {
        Nome = nome;
        Banco = banco;
        SaldoAtual = saldoAtual;
    }
}

 