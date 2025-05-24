
namespace FinancialControl.Application.DTOs
{
    public class ErrorResponseDto
    {
        public int StatusCode { get; set; }
        public string Mensagem { get; set; }
        public string? Detalhes { get; set; }
    }
}
