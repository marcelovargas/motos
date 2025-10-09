using MotoApi.Models;

namespace MotoApi.Repositories.Interfaces;

public interface IMotoRepository
{
    Task<Moto> CreateMotoAsync(Moto moto);
    Task<Moto?> GetMotoByIdAsync(string id);
    Task<bool> MotoExistsByPlacaAsync(string placa);
}