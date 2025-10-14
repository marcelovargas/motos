using MotoApi.Models;

namespace MotoApi.Services.Interfaces
{
    public interface ILocacaoService
    {
        Task<Locacao> CreateLocacaoAsync(Locacao locacao);
        Task<Locacao?> GetByIdAsync(string id);
        Task<(bool success, decimal valorTotal, string? error)> ProcessarDevolucaoAsync(string id, DateTime dataDevolucao);
    }
}