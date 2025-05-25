namespace FinancialControl.Application.DTOs;

public class CartaoRequestDto
{
    public string Nome { get; set; } = null!;
    public decimal LimiteTotal { get; set; }
    public int DiaFechamentoFatura { get; set; }
    public int DiaVencimentoFatura { get; set; }
    public int ContaDePagamentoId { get; set; }
    public int UsuarioId { get; set; }
}