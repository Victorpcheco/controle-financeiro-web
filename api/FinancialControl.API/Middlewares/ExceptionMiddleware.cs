using FinancialControl.Application.DTOs;
using System.Text.Json;

namespace FinancialControl.API.Middlewares
{
    // Middleware para tratamento global de exceções
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next; // Próximo middleware no pipeline
        private readonly ILogger<ExceptionMiddleware> _logger; // Logger para registrar erros
        private readonly IHostEnvironment _env; // Ambiente de hospedagem (desenvolvimento, produção, etc.)
        
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        // Método principal chamado pelo pipeline de requisições
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Tenta executar o próximo middleware
                await _next(context);
            }
            catch (Exception ex)
            {
                // Em caso de exceção, registra o erro e trata a resposta
                _logger.LogError(ex, "Erro não tratado: {Mensagem}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        // Gera a resposta de erro padronizada em JSON
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            // Define o status HTTP conforme o tipo da exceção
            context.Response.StatusCode = exception switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            // Monta o objeto de resposta de erro
            var response = new ErrorResponseDto
            {
                StatusCode = context.Response.StatusCode,
                Mensagem = exception.Message,
                Detalhes = _env.IsDevelopment() ? exception.StackTrace : null // Exibe detalhes apenas em desenvolvimento
            };

            // Serializa e escreve a resposta no corpo da requisição
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}