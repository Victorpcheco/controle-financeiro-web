using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FluentValidation;

namespace FinancialControl.Application.Services
{
    public class CategoriaService(
        ICategoriaRepository repository,
        IMapper mapper,
        IValidator<CategoriaRequestDto> validator
        ) : ICategoriaService
    {
        public async Task<CategoriaPaginadoResponseDto> ListarCategoriaPaginadoAsync(int usuarioId, int pagina, int quantidadePorPagina)
        {
            if (pagina < 1)
                throw new ArgumentException("A página deve ser maior ou igual a 1.");
            if (quantidadePorPagina < 1)
                throw new ArgumentException("A quantidade deve ser maior ou igual a 1.");

            var categorias = await repository.ListarCategoriaPaginadoAsync(usuarioId, pagina, quantidadePorPagina);
            var total = await repository.ContarTotalAsync();
            var categoriaDto = categorias.Select(x => mapper.Map<CategoriaResponseDto>(x)).ToList();
            
            return new CategoriaPaginadoResponseDto
            {
                Total = total,
                Pagina = pagina,
                Quantidade = quantidadePorPagina,
                Categorias = categoriaDto,
            };
        }

        public async Task<Categoria?> ObterCategoriaPorId(int id)
        {
            if (id < 1)
                throw new ArgumentException("Id inválido");

            var categoria = await repository.ObterCategoriaPorIdAsync(id);

            if (categoria == null)
                throw new KeyNotFoundException("Categoria não encontrada.");

            return categoria;
        }

        public async Task<bool> CriarCategoriaAsync(int usuarioId, CategoriaRequestDto dto)
        {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var categoriaExiste = await repository.ObterCategoriaPorNomeAsync(dto.Nome);
            if (categoriaExiste != null)
            {
                throw new InvalidOperationException("Categoria já cadastrada.");
            }
            
            var categoria = mapper.Map<Categoria>(dto);
            categoria.AtualizarUsuarioId(usuarioId);
            
            await repository.CriarCategoriaAsync(categoria);
            return true;
        }

        public async Task<bool> AtualizarCategoriaAsync(int id, CategoriaRequestDto dto)
        {
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var categoria = await repository.ObterCategoriaPorIdAsync(id);
            if (categoria == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }
            
            categoria.AtualizarCategoria(dto.Nome, dto.Tipo);
            
            await repository.AtualizarCategoriaAsync(categoria);
            return true;
        }

        public async Task<bool> DeletarCategoriaAsync(int id)
        {
            if (id < 1)
                throw new ArgumentException("Id inválido");

            var categoria = await repository.ObterCategoriaPorIdAsync(id);
            if (categoria == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }

            await repository.DeletarCategoriaAsync(categoria);
            return true;
        }
    }
}
