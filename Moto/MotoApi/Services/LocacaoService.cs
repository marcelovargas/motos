using MotoApi.Models;
using MotoApi.Services.Interfaces;

namespace MotoApi.Services
{
    public class LocacaoService : ILocacaoService
    {
        public async Task<Locacao> CreateLocacaoAsync(Locacao locacao)
        {
            // In a real application, this would save the locacao to the database
            // For now, we'll just return the locacao as-is after setting any needed defaults
            return await Task.FromResult(locacao);
        }
    }
}