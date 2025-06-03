namespace ControleFinanceiro.Application.Dtos;

public class CartaoResponseDto
{
    public int Id { get; set; }
    public string NomeCartao { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
}