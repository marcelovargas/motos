using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MotoApi.Models;

/// <summary>
/// Represents a motorcycle entity
/// </summary>
public class Moto
{
    /// <summary>
    /// Unique identifier for the motorcycle (string, not empty)
    /// </summary>
    [Key]
    [Required]
    [MinLength(1)]
    public string Identificador { get; set; } = string.Empty;
    
    /// <summary>
    /// Year of the motorcycle
    /// </summary>
    [Required]
    public int Ano { get; set; }
    
    /// <summary>
    /// Model of the motorcycle
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Modelo { get; set; } = string.Empty;
    
    /// <summary>
    /// License plate of the motorcycle (unique)
    /// Format: Should follow format ABC-1234 or ABC-1A11 (3 letters, hyphen, 4 characters)
    /// Total length: 8 characters including the hyphen
    /// </summary>
    [Required]
    [StringLength(8)]
    [RegularExpression(@"^[A-Z]{3}-\d{4}$|^[A-Z]{3}-\d[A-Z]\d{2}$", 
        ErrorMessage = "License plate must follow format: 3 letters + hyphen + 4 numbers (AAA-1111) or 3 letters + hyphen + number + letter + 2 numbers (AAA-1A11)")]
    [JsonPropertyName("placa")]
    public string Placa { get; set; } = string.Empty;
    
    /// <summary>
    /// Navigation property for locacoes
    /// </summary>
    public virtual ICollection<Locacao> Locacoes { get; set; } = new List<Locacao>();
}