using MotoApi.Models;
using MotoApi.Repositories.Interfaces;
using MotoApi.Services.Interfaces;

namespace MotoApi.Services
{
    public class LocacaoService : ILocacaoService
    {
        private readonly ILocacaoRepository _locacaoRepository;

        public LocacaoService(ILocacaoRepository locacaoRepository)
        {
            _locacaoRepository = locacaoRepository;
        }

        public async Task<Locacao> CreateLocacaoAsync(Locacao locacao)
        {
            return await _locacaoRepository.CreateLocacaoAsync(locacao);
        }
    }
}