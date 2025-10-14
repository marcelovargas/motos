using MotoApi.Data;
using MotoApi.Models;
using MotoApi.Repositories.Interfaces;
using MotoApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MotoApi.Services
{
    public class LocacaoService : ILocacaoService
    {
        private readonly ILocacaoRepository _locacaoRepository;
        private readonly IEntregadorRepository _entregadorRepository;
        private readonly IMotoRepository _motoRepository;
        private readonly ApplicationDbContext _context;

        public LocacaoService(ILocacaoRepository locacaoRepository, 
                              IEntregadorRepository entregadorRepository,
                              IMotoRepository motoRepository,
                              ApplicationDbContext context)
        {
            _locacaoRepository = locacaoRepository;
            _entregadorRepository = entregadorRepository;
            _motoRepository = motoRepository;
            _context = context;
        }

        public async Task<Locacao> CreateLocacaoAsync(Locacao locacao)
        {
            // Regra: A locação obrigatóriamente tem que ter uma data de inicio e uma data de previsão de término.
            if (locacao.DataInicio == default || locacao.DataPrevisaoTermino == default)
            {
                throw new ArgumentException("Data de início e data de previsão de término são obrigatórias.");
            }

            // Regra: O inicio da locação obrigatóriamente é o primeiro dia após a data de criação.
            var dataAtual = DateTime.UtcNow.Date;
            var dataInicioEsperada = dataAtual.AddDays(1);
            if (locacao.DataInicio.Date != dataInicioEsperada)
            {
                throw new ArgumentException($"A data de início deve ser o primeiro dia após a data de criação (esperado: {dataInicioEsperada:yyyy-MM-dd}, recebido: {locacao.DataInicio:yyyy-MM-dd}).");
            }

            // Regra: Somente entregadores habilitados na categoria A podem efetuar uma locação
            var entregador = await _entregadorRepository.GetByIdAsync(locacao.EntregadorId);
            if (entregador == null)
            {
                throw new ArgumentException("Entregador não encontrado.");
            }

            if (entregador.TipoCnh != "A")
            {
                throw new ArgumentException("Somente entregadores com habilitação na categoria A podem efetuar locação.");
            }

            // Regra: Os planos disponíveis para locação
            if (!PlanosLocacao.IsValidPlano(locacao.Plano))
            {
                throw new ArgumentException($"Plano inválido. Planos válidos: {string.Join(", ", PlanosLocacao.Disponiveis.Select(p => p.Dias))} dias.");
            }

            // Regra: A data de previsão de término deve ser igual à data de início mais o número de dias do plano
            var dataPrevisaoTerminoCalculada = locacao.DataInicio.AddDays(locacao.Plano);
            if (locacao.DataPrevisaoTermino.Date != dataPrevisaoTerminoCalculada.Date)
            {
                throw new ArgumentException($"A data de previsão de término deve ser igual à data de início mais o número de dias do plano (esperado: {dataPrevisaoTerminoCalculada:yyyy-MM-dd}, recebido: {locacao.DataPrevisaoTermino:yyyy-MM-dd}).");
            }

            // Regra: Se data de término for fornecida, ela deve ser igual ou posterior à data de início
            if (locacao.DataTermino.HasValue && locacao.DataTermino < locacao.DataInicio)
            {
                throw new ArgumentException("A data de término deve ser igual ou posterior à data de início.");
            }

            // Regra: Verificar se a moto está disponível para locação (não está atualmente locada)
            var locacoesAtivas = await _context.Locacoes
                .Where(l => l.MotoId == locacao.MotoId && 
                           l.DataInicio <= locacao.DataInicio && 
                           (!l.DataTermino.HasValue || l.DataTermino >= locacao.DataInicio))
                .ToListAsync();

            if (locacoesAtivas.Any())
            {
                throw new ArgumentException("A moto já está locada para o período solicitado.");
            }

            return await _locacaoRepository.CreateLocacaoAsync(locacao);
        }

        public async Task<Locacao?> GetByIdAsync(string id)
        {
            return await _locacaoRepository.GetByIdAsync(id);
        }
    }
}