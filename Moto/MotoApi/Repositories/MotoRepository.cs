using Microsoft.EntityFrameworkCore;
using MotoApi.Data;
using MotoApi.Models;
using MotoApi.Repositories.Interfaces;

namespace MotoApi.Repositories;

public class MotoRepository : IMotoRepository
{
    private readonly ApplicationDbContext _context;

    public MotoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Moto> CreateMotoAsync(Moto moto)
    {
        _context.Motos.Add(moto);
        await _context.SaveChangesAsync();
        return moto;
    }

    public async Task<Moto?> GetMotoByIdAsync(string id)
    {
        return await _context.Motos.FindAsync(id);
    }

    public async Task<bool> MotoExistsByPlacaAsync(string placa)
    {
        return await _context.Motos.AnyAsync(m => m.Placa == placa);
    }
}