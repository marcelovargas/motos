using System.ComponentModel.DataAnnotations;

namespace MotoApi.Models
{
    public class PlanoLocacao
    {
        public int Dias { get; set; }
        public decimal ValorPorDia { get; set; }
    }

    public static class PlanosLocacao
    {
        public static readonly List<PlanoLocacao> Disponiveis = new List<PlanoLocacao>
        {
            new PlanoLocacao { Dias = 7, ValorPorDia = 30.00m },
            new PlanoLocacao { Dias = 15, ValorPorDia = 28.00m },
            new PlanoLocacao { Dias = 30, ValorPorDia = 22.00m },
            new PlanoLocacao { Dias = 45, ValorPorDia = 20.00m },
            new PlanoLocacao { Dias = 50, ValorPorDia = 18.00m }
        };

        public static bool IsValidPlano(int planoDias)
        {
            return Disponiveis.Any(p => p.Dias == planoDias);
        }

        public static decimal GetValorPorDia(int planoDias)
        {
            var plano = Disponiveis.FirstOrDefault(p => p.Dias == planoDias);
            return plano?.ValorPorDia ?? 0;
        }
    }
}