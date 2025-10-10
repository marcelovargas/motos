using MotoApi.Models;

namespace MotoApi.Services.Interfaces;

public interface IEntregadorService
{
    Task<Entregador> CreateEntregadorAsync(Entregador entregador);
}