using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.MesDeReferencia.AtualizarMesReferencia;

public class AtualizarMesReferenciaUseCase(IMesReferenciaRepository repository, IValidator<MesReferenciaCriarDto> validator) : IAtualizarMesReferenciaUseCase
{
    public async Task<bool> ExecuteAsync(int id, MesReferenciaCriarDto requestDto)
    {
        var validationReSult = await validator.ValidateAsync(requestDto);
        if (!validationReSult.IsValid)
            throw new ValidationException(validationReSult.Errors);
        
        if (id <= 0) throw new ArgumentException("ID inválido.");
        
        var mesReferencia = await repository.BuscarMesReferenciaPorId(id);
        if (mesReferencia == null)
            throw new ArgumentException("Mês de referência não encontrado.");
        
        mesReferencia.AtualizarMes(requestDto.NomeMes);
        await repository.AtualizarMesReferenciaAsync(mesReferencia);
        return true;
    }
}