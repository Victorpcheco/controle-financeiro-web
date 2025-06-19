using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Financeiro.ListarSaldosContas
{
    public class ListarContasComSaldoTotalUseCase(IFinanceiroRepository repository, IUserContext userContext) : IListarContasComSaldoTotalUseCase
    {
        public async Task<ListarSaldosContasResponse> ExecutarAsync()
        {
            try
            {
                var usuarioId = userContext.UsuarioId;

                var saldos = await repository.ListarContasComSaldoTotalAsync(usuarioId);

                var saldosDto = saldos.Select(s => new SaldoContaDto
                {
                    Id = s.ContaId,
                    NomeConta = s.NomeConta,
                    SaldoAtual = s.SaldoAtual
                }).ToList();

                return new ListarSaldosContasResponse
                {
                    Sucesso = true,
                    Mensagem = $"{saldosDto.Count} conta(s) encontrada(s)",
                    Dados = saldosDto
                };
            }
            catch (Exception ex)
            {
                return new ListarSaldosContasResponse
                {
                    Sucesso = false,
                    Mensagem = "Erro interno do servidor",
                    Dados = new List<SaldoContaDto>()
                };
            }
        }
    }
}
