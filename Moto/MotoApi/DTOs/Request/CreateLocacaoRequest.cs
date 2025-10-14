using System.ComponentModel.DataAnnotations;

namespace MotoApi.DTOs.Request
{
    /// <summary>
    /// DTO for creating a new locação
    /// </summary>
    public class CreateLocacaoRequest
    {
        /// <summary>
        /// ID of the entregador
        /// </summary>
        /// <example>entregador123</example>
        [Required]
        public string entregador_id { get; set; } = string.Empty;

        /// <summary>
        /// ID of the moto
        /// </summary>
        /// <example>moto123</example>
        [Required]
        public string moto_id { get; set; } = string.Empty;

        /// <summary>
        /// Start date of the rental
        /// </summary>
        /// <example>2024-01-01T00:00:00Z</example>
        [Required]
        public DateTime data_inicio { get; set; }

        /// <summary>
        /// End date of the rental (optional)
        /// </summary>
        /// <example>2024-01-07T23:59:59Z</example>
        public DateTime? data_termino { get; set; }

        /// <summary>
        /// Expected end date of the rental
        /// </summary>
        /// <example>2024-01-07T23:59:59Z</example>
        [Required]
        public DateTime data_previsao_termino { get; set; }

        /// <summary>
        /// Rental plan (number of days)
        /// </summary>
        /// <example>7</example>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Plano must be greater than 0")]
        public int plano { get; set; }
    }
}