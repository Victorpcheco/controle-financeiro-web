using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.DTOs
{
    public class MovimentacoesDashboardPaginadoResponseDto
    {
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int Quantidade { get; set; }
        public List<MovimentacoesResumo> Movimentacoes { get; set; } = null!;
    }
}
