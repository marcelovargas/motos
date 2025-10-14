using System.ComponentModel.DataAnnotations;

namespace MotoApi.Models
{
    public class EventoMoto2024
    {
        [Key]
        public int Id { get; set; }
        
        public string Identificador { get; set; } = string.Empty;
        public int Ano { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public DateTime DataRegistroEvento { get; set; }
        public DateTime DataRecebimento { get; set; }
    }
}