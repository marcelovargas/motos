using MotoApi.Models;

namespace MotoApi.Repositories.Interfaces
{
    public interface ILocacaoRepository
    {
        Task<Locacao> CreateLocacaoAsync(Locacao locacao);
        Task<Locacao?> GetByIdAsync(string id);
        Task<bool> UpdateDataTerminoAsync(string id, DateTime dataTermino);
    }
}