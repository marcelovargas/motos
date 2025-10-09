using System.ComponentModel.DataAnnotations;

namespace MotoApi.Models;

public class Moto
{
    [Key]
    public int Identificador { get; set; }
    
    [Required]
    public int Ano { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Modelo { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    public string Placa { get; set; } = string.Empty;
}