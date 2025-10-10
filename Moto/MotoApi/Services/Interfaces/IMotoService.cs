using MotoApi.Models;

namespace MotoApi.Services.Interfaces;

public interface IMotoService
{
    Task<Moto> CreateMotoAsync(Moto moto);
    Task<Moto?> GetMotoByIdAsync(string id);
    Task<IEnumerable<Moto>> GetMotosAsync(string? placa = null);
    Task<bool> DeleteMotoAsync(string id);
}