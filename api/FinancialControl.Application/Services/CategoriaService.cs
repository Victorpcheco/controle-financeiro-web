using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FluentValidation;

namespace FinancialControl.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoriaRequestDto> _categoriaValidator;

        public CategoriaService(ICategoriaRepository categoriaRepository, IMapper mapper,
            IValidator<CategoriaRequestDto> categoriaValidator)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
            _categoriaValidator = categoriaValidator;
        }

        public async Task<CategoriaPaginadoResponse> ListarPaginadoAsync(int usuarioId, int pagina, int quantidadePorPagina)
        {
            if (pagina < 1)
                throw new ArgumentException("A página deve ser maior ou igual a 1.");
            if (quantidadePorPagina < 1)
                throw new ArgumentException("A quantidade deve ser maior ou igual a 1.");

            var categorias = await _categoriaRepository.ListarPaginadoAsync(usuarioId, pagina, quantidadePorPagina);
            var categoriaDto = categorias.Select(x => _mapper.Map<CategoriaResponseDto>(x)).ToList();
            var total = await _categoriaRepository.ContarTotalAsync();

            return new CategoriaPaginadoResponse
            {
                Total = total,
                Pagina = pagina,
                Quantidade = quantidadePorPagina,
                Categorias = categoriaDto,
            };
        }

        public async Task<Categoria?> BuscarPorIdAsync(int id)
        {
            if (id < 1)
                throw new ArgumentException("Id inválido");

            var categoria = await _categoriaRepository.BuscarPorId(id);

            if (categoria == null)
                throw new KeyNotFoundException($"Categoria com id {id} não encontrada.");

            return categoria;
        }

        public async Task<bool> CriarCategoriaAsync(CategoriaRequestDto dto)
        {
            var validationResult = await _categoriaValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var categoria = _mapper.Map<Categoria>(dto);

            var categoriaExists = await _categoriaRepository.BuscarPorId(categoria.Id);
            if (categoriaExists != null)
            {
                throw new InvalidOperationException($"Categoria com id {categoria.Id} já existe.");
            }

            await _categoriaRepository.CriarCategoriaAsync(categoria);
            return true;
        }

        public async Task<bool> AtualizarCategoriaAsync(int id, CategoriaRequestDto dto)
        {
            var validationResult = await _categoriaValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var categoria = _mapper.Map<Categoria>(dto);

            var categoriaExists = await _categoriaRepository.BuscarPorId(id);
            if (categoriaExists == null)
            {
                throw new KeyNotFoundException($"Categoria com id {categoria.Id} não encontrada.");
            }
            
            categoriaExists.Nome = dto.Nome;
            categoriaExists.Tipo = dto.Tipo;
            
            await _categoriaRepository.AtualizarCategoriaAsync(categoriaExists);
            return true;
        }

        public async Task<bool> DeletarCategoriaAsync(int id)
        {
            if (id < 1)
                throw new ArgumentException("Id inválido");

            var categoria = await _categoriaRepository.BuscarPorId(id);
            if (categoria == null)
            {
                throw new KeyNotFoundException($"Categoria com id {id} não encontrada.");
            }

            await _categoriaRepository.DeletarCategoriaAsync(categoria);
            return true;
        }
    }
}
