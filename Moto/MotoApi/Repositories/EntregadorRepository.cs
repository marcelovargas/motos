using Microsoft.EntityFrameworkCore;
using MotoApi.Data;
using MotoApi.Models;
using MotoApi.Repositories.Interfaces;

namespace MotoApi.Repositories;

public class EntregadorRepository : IEntregadorRepository
{
    private readonly ApplicationDbContext _context;

    public EntregadorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Entregador> CreateEntregadorAsync(Entregador entregador)
    {
        _context.Entregadores.Add(entregador);
        await _context.SaveChangesAsync();
        return entregador;
    }

    public async Task<bool> EntregadorExistsByCnpjAsync(string cnpj)
    {
        return await _context.Entregadores.AnyAsync(e => e.Cnpj == cnpj);
    }

    public async Task<bool> EntregadorExistsByNumeroCnhAsync(string numeroCnh)
    {
        return await _context.Entregadores.AnyAsync(e => e.NumeroCnh == numeroCnh);
    }
}