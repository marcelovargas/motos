using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MotoApi.DTOs.Response
{
    /// <summary>
    /// DTO for moto response
    /// </summary>
    public class MotoResponseDto
    {
        /// <summary>
        /// Unique identifier for the motorcycle
        /// </summary>
        /// <example>moto123</example>
        [JsonPropertyName("identificador")]
        public string Identificador { get; set; } = string.Empty;
        
        /// <summary>
        /// Year of the motorcycle
        /// </summary>
        /// <example>2020</example>
        [JsonPropertyName("ano")]
        public int Ano { get; set; }
        
        /// <summary>
        /// Model of the motorcycle
        /// </summary>
        /// <example>Mottu Sport</example>
        [JsonPropertyName("modelo")]
        public string Modelo { get; set; } = string.Empty;
        
        /// <summary>
        /// License plate of the motorcycle
        /// </summary>
        /// <example>CDX-0101</example>
        [JsonPropertyName("placa")]
        public string Placa { get; set; } = string.Empty;
    }
}