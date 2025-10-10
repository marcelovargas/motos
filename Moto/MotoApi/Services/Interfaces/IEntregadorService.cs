using MotoApi.Models;

namespace MotoApi.Services.Interfaces;

public interface IEntregadorService
{
    Task<Entregador> CreateEntregadorAsync(Entregador entregador);
    Task<bool> UpdateEntregadorCnhImageAsync(string id, string cnhImagePath);
}