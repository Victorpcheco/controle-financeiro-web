namespace FinancialControl.Application.DTOs;

public class CartaoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public decimal LimiteTotal { get; set; }
    public decimal LimiteDisponivel { get; set; }
    public string DiaFechamentoFatura { get; set; } = null!;
    public string DiaVencimentoFatura { get; set; } = null!;
    public int ContaDePagamentoId { get; set; }
    public int UsuarioId { get; set; }
}