using System.Text.Json.Serialization;

namespace MotoApi.DTOs.Response
{
    public class GetLocacaoResponse
    {
        [JsonPropertyName("identificador")]
        public string Identificador { get; set; } = string.Empty;
        
        [JsonPropertyName("valor_diaria")]
        public decimal ValorDiaria { get; set; }
        
        [JsonPropertyName("entregador_id")]
        public string EntregadorId { get; set; } = string.Empty;
        
        [JsonPropertyName("moto_id")]
        public string MotoId { get; set; } = string.Empty;
        
        [JsonPropertyName("data_inicio")]
        public DateTime DataInicio { get; set; }
        
        [JsonPropertyName("data_termino")]
        public DateTime? DataTermino { get; set; }
        
        [JsonPropertyName("data_previsao_termino")]
        public DateTime DataPrevisaoTermino { get; set; }
        
        [JsonPropertyName("data_devolucao")]
        public DateTime? DataDevolucao { get; set; }
    }
}