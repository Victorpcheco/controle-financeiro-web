namespace ControleFinanceiro.Application.Dtos;

public class DespesaCriarDto
{
    public string TituloDespesa { get;  set; } = null!;
    public DateOnly Data { get;  set; }
    public int MesReferenciaId { get;  set; }
    public int CategoriaId { get;  set; }
    public int CartaoId { get;  set; }
    public int ContaBancariaId { get;  set; }
    public decimal Valor { get;  set; }
    public bool Realizado { get;  set; } = false;
}