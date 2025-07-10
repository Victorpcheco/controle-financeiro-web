namespace ControleFinanceiro.Application.Dtos;

public class MesReferenciaPaginadoResponseDto
{
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int Quantidade { get; set; }
    public IReadOnlyList<MesReferenciaResponseDto?> Meses { get; set; } = null!;
}