using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Contas;

public interface IListarContasBancarias
{
    Task<ContaPaginadoResponse> ListarContasPaginado(int pagina, int quantidadePorPagina);
}