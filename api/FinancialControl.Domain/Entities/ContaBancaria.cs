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
    
    public void CriarContaBancaria(string nome, string banco, decimal saldoInicial)
    {
        Nome = nome;
        Banco = banco;
        SaldoInicial = saldoInicial;
        SaldoAtual = saldoInicial;
    }
    
}

 