using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MotoApi.Models;

/// <summary>
/// Represents an entregador (delivery person) entity
/// </summary>
public class Entregador
{
    /// <summary>
    /// Unique identifier for the entregador (string, not empty)
    /// </summary>
    [Key]
    [Required]
    [MinLength(1)]
    public string Identificador { get; set; } = string.Empty;
    
    /// <summary>
    /// Name of the entregador (not empty)
    /// </summary>
    [Required]
    [MinLength(1)]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// CNPJ of the entregador (unique)
    /// Format: 00000000000000 (14 digits without formatting)
    /// </summary>
    [Required]
    [StringLength(14)]
    [RegularExpression(@"^\d{14}$", 
        ErrorMessage = "CNPJ must be 14 digits in the format 00000000000000")]
    public string Cnpj { get; set; } = string.Empty;
    
    /// <summary>
    /// Birth date of the entregador
    /// </summary>
    [Required]
    public DateTime DataNascimento { get; set; }
    
    /// <summary>
    /// CNH number of the entregador (unique)
    /// </summary>
    [Required]
    [StringLength(20)]
    public string NumeroCnh { get; set; } = string.Empty;
    
    /// <summary>
    /// CNH type of the entregador (A, B or A+B)
    /// </summary>
    [Required]
    [RegularExpression(@"^(A|B|A\+B)$", 
        ErrorMessage = "CNH type must be A, B or A+B")]
    public string TipoCnh { get; set; } = string.Empty;
    
    /// <summary>
    /// Image of the CNH
    /// </summary>
    
    public string ImagemCnh { get; set; } = string.Empty;
}