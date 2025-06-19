namespace ControleFinanceiro.Application.Dtos;

public class ReceitaResponseDto
{
    public int Id { get; set; }
    public string TituloReceita { get; set; } = null!;
    public DateOnly Data { get; set; }
    public int MesReferenciaId { get; set; }
    public int CategoriaId { get; set; }
    public int ContaBancariaId { get; set; }
    public decimal Valor { get; set; }
    public bool Realizado { get; set; }
}