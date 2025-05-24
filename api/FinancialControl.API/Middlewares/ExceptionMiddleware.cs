using FinancialControl.Application.DTOs;
using System.Text.Json;

namespace FinancialControl.API.Middlewares
{
    public class ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
    {
    // Método principal chamado pelo pipeline de requisições
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Tenta executar o próximo middleware (se houver)
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro não tratado: {Mensagem}", ex.Message);
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
            Detalhes = env.IsDevelopment() ? exception.StackTrace : null 
        };

        // Serializa e escreve a resposta no corpo da requisição
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    }
}