using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.MesDeReferencia.CriarMesReferencia;

public class CriarMesReferenciaUseCase(IMesReferenciaRepository repository, IValidator<MesReferenciaCriarDto> validator, IUserContext context) : ICriarMesReferenciaUseCase
{
    public async Task<bool> ExecuteAsync(MesReferenciaCriarDto requestDto)
    {
        var validationResult = await validator.ValidateAsync(requestDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var usuarioId = context.UsuarioId;
        
        var mes = new MesReferencia(requestDto.NomeMes, usuarioId);
        await repository.AdicionarMesReferenciaAsync(mes);

        return true;
    }
}