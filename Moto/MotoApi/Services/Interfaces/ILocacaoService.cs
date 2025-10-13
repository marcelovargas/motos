using MotoApi.Models;

namespace MotoApi.Services.Interfaces
{
    public interface ILocacaoService
    {
        Task<Locacao> CreateLocacaoAsync(Locacao locacao);
    }
}