using MotoApi.Models;
using System.Linq.Expressions;

namespace MotoApi.Repositories.Interfaces;

public interface IMotoRepository
{
    Task<Moto> CreateMotoAsync(Moto moto);
    Task<Moto?> GetMotoByIdAsync(string id);
    Task<bool> MotoExistsByPlacaAsync(string placa);
    Task<IEnumerable<Moto>> GetMotosAsync(Expression<Func<Moto, bool>>? filter = null);
    Task<bool> DeleteMotoAsync(string id);
}