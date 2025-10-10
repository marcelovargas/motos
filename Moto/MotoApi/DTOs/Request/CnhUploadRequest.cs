using System.ComponentModel.DataAnnotations;

namespace MotoApi.DTOs.Request
{
    /// <summary>
    /// DTO for CNH image upload request
    /// </summary>
    public class CnhUploadRequest
    {
        /// <summary>
        /// Base64 encoded image string
        /// </summary>
       
        [Required]
        public string Imagem_cnh { get; set; } = string.Empty;
    }
}