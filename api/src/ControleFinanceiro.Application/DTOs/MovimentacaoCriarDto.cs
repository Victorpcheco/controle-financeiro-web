using ControleFinanceiro.Domain.Enum;

namespace ControleFinanceiro.Application.Dtos;

public class MovimentacaoCriarDto
{
    public string Titulo { get; set; } = null!;
    public DateOnly DataVencimento { get; set; }
    public Tipo Tipo { get; set; }
    public int MesReferenciaId { get; set; }
    public int CategoriaId { get; set; }
    public int? CartaoId { get; set; }
    public int ContaBancariaId { get; set; }
    public decimal Valor { get; set; }
    public bool Realizado { get; set; } = false;
    public int UsuarioId { get; set; }
    public string? FormaDePagamento { get; set; }
}