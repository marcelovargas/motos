using System;
using System.Text.Json.Serialization;

namespace MotoApi.Models
{
    public class Locacao
    {
        [JsonPropertyName("entregador_id")]
        public string EntregadorId { get; set; }
        
        [JsonPropertyName("moto_id")]
        public string MotoId { get; set; }
        
        [JsonPropertyName("data_inicio")]
        public DateTime DataInicio { get; set; }
        
        [JsonPropertyName("data_termino")]
        public DateTime DataTermino { get; set; }
        
        [JsonPropertyName("data_previsao_termino")]
        public DateTime DataPrevisaoTermino { get; set; }
        
        [JsonPropertyName("plano")]
        public int Plano { get; set; }
    }
}