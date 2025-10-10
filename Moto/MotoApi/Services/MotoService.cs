using MotoApi.Models;
using MotoApi.Repositories.Interfaces;
using MotoApi.Services.Interfaces;
using System.Linq.Expressions;

namespace MotoApi.Services;

public class MotoService : IMotoService
{
    private readonly IMotoRepository _motoRepository;

    public MotoService(IMotoRepository motoRepository)
    {
        _motoRepository = motoRepository;
    }

    public async Task<Moto> CreateMotoAsync(Moto moto)
    {
        // Check if a moto with the same plate already exists
        if (await _motoRepository.MotoExistsByPlacaAsync(moto.Placa))
        {
            throw new ArgumentException($"A moto with plate {moto.Placa} already exists.");
        }

        return await _motoRepository.CreateMotoAsync(moto);
    }

    public async Task<Moto?> GetMotoByIdAsync(string id)
    {
        return await _motoRepository.GetMotoByIdAsync(id);
    }

    public async Task<IEnumerable<Moto>> GetMotosAsync(string? placa = null)
    {
        Expression<Func<Moto, bool>>? filter = null;

        if (!string.IsNullOrEmpty(placa))
        {
            filter = m => m.Placa == placa;
        }

        return await _motoRepository.GetMotosAsync(filter);
    }

    public async Task<bool> UpdateMotoPlacaAsync(string id, string placa)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(placa))
        {
            throw new ArgumentException("Dados inválidos");
        }

        
        var regex = new System.Text.RegularExpressions.Regex(@"^[A-Z]{3}-\d{4}$|^[A-Z]{3}-\d[A-Z]\d{2}$");
        if (!regex.IsMatch(placa))
        {
            throw new ArgumentException("Dados inválidos");
        }

        // Check if another moto already has this plate
        if (await _motoRepository.MotoExistsByPlacaAsync(placa))
        {
            throw new ArgumentException("Dados inválidos");
        }

        return await _motoRepository.UpdateMotoPlacaAsync(id, placa);
    }

    public async Task<bool> DeleteMotoAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException("Invalid identifier");
        }

        return await _motoRepository.DeleteMotoAsync(id);
    }
}