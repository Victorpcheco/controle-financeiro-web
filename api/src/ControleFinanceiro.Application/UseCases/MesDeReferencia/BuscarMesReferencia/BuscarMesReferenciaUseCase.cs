using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.MesDeReferencia.BuscarMesReferencia;

public class BuscarMesReferenciaUseCase(IMesReferenciaRepository repository) : IBuscarMesReferenciaUseCase
{
    public async Task<MesReferenciaResponseDto?> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O ID deve ser um número positivo.");
        
        var mes = await repository.BuscarMesReferenciaPorId(id);
        if (mes == null)
            throw new ArgumentException("Mês de referência não encontrado.");
        
        return new MesReferenciaResponseDto
        {
            Id = mes.Id,
            NomeMes = mes.NomeMes,
            UsuarioId = mes.UsuarioId
        };
    }
}