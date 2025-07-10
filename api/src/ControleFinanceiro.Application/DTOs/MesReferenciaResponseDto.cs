namespace ControleFinanceiro.Application.Dtos;

public class MesReferenciaResponseDto
{
    public int Id { get; set; }
    public string NomeMes { get; set; } = null!;
    public int UsuarioId { get; set; }
}