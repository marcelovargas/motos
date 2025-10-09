using MotoApi.Models;
using MotoApi.Repositories.Interfaces;
using MotoApi.Services.Interfaces;

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
}