namespace MotoApi.Models
{
    public class MotoCadastradaEvent
    {
        public string Identificador { get; set; } = string.Empty;
        public int Ano { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public DateTime DataRegistro { get; set; }
    }
}