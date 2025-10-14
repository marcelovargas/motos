using System;
using System.Text.Json.Serialization;

namespace MotoApi.Models
{
    public class Locacao
    {
        [JsonPropertyName("identificador")]
        public string Identificador { get; set; }
        
        [JsonPropertyName("entregador_id")]
        public string EntregadorId { get; set; }
        
        [JsonPropertyName("moto_id")]
        public string MotoId { get; set; }
        
        [JsonPropertyName("data_inicio")]
        public DateTime DataInicio { get; set; }
        
        [JsonPropertyName("data_termino")]
        public DateTime? DataTermino { get; set; }
        
        [JsonPropertyName("data_previsao_termino")]
        public DateTime DataPrevisaoTermino { get; set; }
        
        [JsonPropertyName("plano")]
        public int Plano { get; set; }
        
        // Navigation properties
        public virtual Entregador Entregador { get; set; }
        public virtual Moto Moto { get; set; }

        public decimal CalcularValorTotal(DateTime dataDevolucao)
        {
            var diasContratados = Plano;
            var valorDiaria = PlanosLocacao.GetValorPorDia(Plano);
            var dataPrevistaDevolucao = DataInicio.AddDays(diasContratados);
            
            
            var diasUsados = (dataDevolucao.Date - DataInicio.Date).Days + 1;
            
            if (dataDevolucao.Date < DataPrevisaoTermino.Date)
            {
                
                var valorBase = diasUsados * valorDiaria;
                
                
                if (Plano == 7)
                {
                    var diasNaoUsados = diasContratados - diasUsados;
                    var multa = (diasNaoUsados * valorDiaria) * 0.20m; // 20% sobre diárias não efetivadas
                    return valorBase + multa;
                }
                else if (Plano == 15)
                {
                    var diasNaoUsados = diasContratados - diasUsados;
                    var multa = (diasNaoUsados * valorDiaria) * 0.40m; // 40% sobre diárias não efetivadas
                    return valorBase + multa;
                }
                else
                {
                    
                    return valorBase;
                }
            }
            else if (dataDevolucao.Date > DataPrevisaoTermino.Date)
            {
              
                var valorBase = diasContratados * valorDiaria;
                var diasAdicionais = (dataDevolucao.Date - DataPrevisaoTermino.Date).Days;
                var valorAdicional = diasAdicionais * 50.00m; 
                return valorBase + valorAdicional;
            }
            else
            {
                
                return diasContratados * valorDiaria;
            }
        }

        public int ObterDiasUsados(DateTime dataDevolucao)
        {
            return (dataDevolucao.Date - DataInicio.Date).Days + 1;
        }
    }
}