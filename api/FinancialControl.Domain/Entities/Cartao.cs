using System.ComponentModel.DataAnnotations;

namespace FinancialControl.Domain.Entities;

public class Cartao
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = null!;
    [Required(ErrorMessage = "O limite do cartão é obrigatório.")]
    public decimal LimiteTotal { get; set; }
    public decimal LimiteDisponivel { get; set; }
    [Required(ErrorMessage = "A data de fechamento da fatura é obrigatória.")]
    public string DiaFechamentoFatura { get; set; } = null!;
    [Required(ErrorMessage = "A data de vencimento da fatura é obrigatória.")]
    public string DiaVencimentoFatura { get; set; } = null!;
    public int ContaDePagamentoId { get; set; }
    public ContaBancaria ContaDePagamento { get; set; } = null!;
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    
}