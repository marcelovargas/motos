using MotoApi.Models;

namespace MotoApi.Repositories.Interfaces;

public interface IEntregadorRepository
{
    Task<Entregador> CreateEntregadorAsync(Entregador entregador);
    Task<Entregador?> GetEntregadorByIdAsync(string id);
    Task<Entregador?> GetByIdAsync(string id);
    Task<bool> EntregadorExistsByCnpjAsync(string cnpj);
    Task<bool> EntregadorExistsByNumeroCnhAsync(string numeroCnh);
    Task<bool> UpdateEntregadorCnhImageAsync(string id, string cnhImagePath);
}