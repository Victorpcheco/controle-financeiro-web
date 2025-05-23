namespace FinancialControl.Application.DTOs;

public class CartaoRequestDto
{
    public string Nome { get; set; } = null!;
    public decimal LimiteTotal { get; set; }
    public string? DiaFechamentoFatura { get; set; }
    public string? DiaVencimentoFatura { get; set; }

}