namespace ControleFinanceiro.Application.Dtos;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Mensagem { get; set; } = null!;
    public string? Detalhes { get; set; }
}