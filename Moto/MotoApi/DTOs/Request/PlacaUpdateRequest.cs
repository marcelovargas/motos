using System.ComponentModel.DataAnnotations;

namespace MotoApi.DTOs.Request
{
    /// <summary>
    /// DTO for updating moto plate request
    /// </summary>
    public class PlacaUpdateRequest
    {
        /// <summary>
        /// New plate number for the moto
        /// </summary>
        /// <example>ABC-1234</example>
        [Required]
        public string Placa { get; set; } = string.Empty;
    }
}