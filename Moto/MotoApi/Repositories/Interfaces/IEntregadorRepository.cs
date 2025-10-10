using MotoApi.Models;

namespace MotoApi.Repositories.Interfaces;

public interface IEntregadorRepository
{
    Task<Entregador> CreateEntregadorAsync(Entregador entregador);
    Task<bool> EntregadorExistsByCnpjAsync(string cnpj);
    Task<bool> EntregadorExistsByNumeroCnhAsync(string numeroCnh);
}