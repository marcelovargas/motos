using System.ComponentModel.DataAnnotations;

namespace MotoApi.DTOs.Request
{
    public class DevolucaoLocacaoRequest
    {
        [Required]
        public DateTime data_devolucao { get; set; }
    }
}