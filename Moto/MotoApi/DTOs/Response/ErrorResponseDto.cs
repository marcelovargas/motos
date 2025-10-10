using System.ComponentModel.DataAnnotations;

namespace MotoApi.DTOs.Response
{
    /// <summary>
    /// DTO para respostas de erro da API
    /// </summary>
    public class ErrorResponseDto
    {
        /// <summary>
        /// Mensagem de erro descritiva
        /// </summary>
        /// <example>Moto não encontrada</example>
        [Required]
        public string mensagem { get; set; } = string.Empty;
    }
}
