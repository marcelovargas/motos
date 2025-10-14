using Microsoft.EntityFrameworkCore;
using MotoApi.Data;
using MotoApi.Models;
using MotoApi.Repositories.Interfaces;

namespace MotoApi.Repositories
{
    public class LocacaoRepository : ILocacaoRepository
    {
        private readonly ApplicationDbContext _context;

        public LocacaoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Locacao> CreateLocacaoAsync(Locacao locacao)
        {
            // Generate a unique identifier if not provided
            if (string.IsNullOrEmpty(locacao.Identificador))
            {
                locacao.Identificador = Guid.NewGuid().ToString();
            }

            _context.Locacoes.Add(locacao);
            await _context.SaveChangesAsync();

            return await _context.Locacoes
                .Include(l => l.Entregador)
                .Include(l => l.Moto)
                .FirstOrDefaultAsync(l => l.Identificador == locacao.Identificador);
        }

        public async Task<Locacao?> GetByIdAsync(string id)
        {
            return await _context.Locacoes
                .Include(l => l.Entregador)
                .Include(l => l.Moto)
                .FirstOrDefaultAsync(l => l.Identificador == id);
        }

        public async Task<bool> UpdateDataTerminoAsync(string id, DateTime dataTermino)
        {
            var locacao = await _context.Locacoes.FirstOrDefaultAsync(l => l.Identificador == id);
            if (locacao == null)
            {
                return false;
            }

            locacao.DataTermino = dataTermino;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}