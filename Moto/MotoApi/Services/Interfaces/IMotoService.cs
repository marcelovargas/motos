using MotoApi.Models;

namespace MotoApi.Services.Interfaces;

public interface IMotoService
{
    Task<Moto> CreateMotoAsync(Moto moto);
    Task<Moto?> GetMotoByIdAsync(string id);
}