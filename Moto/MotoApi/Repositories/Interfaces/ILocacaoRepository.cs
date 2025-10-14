using MotoApi.Models;

namespace MotoApi.Repositories.Interfaces
{
    public interface ILocacaoRepository
    {
        Task<Locacao> CreateLocacaoAsync(Locacao locacao);

    }
}