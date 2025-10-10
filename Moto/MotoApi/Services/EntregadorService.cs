using MotoApi.Models;
using MotoApi.Repositories.Interfaces;
using MotoApi.Services.Interfaces;

namespace MotoApi.Services;

public class EntregadorService : IEntregadorService
{
    private readonly IEntregadorRepository _entregadorRepository;

    public EntregadorService(IEntregadorRepository entregadorRepository)
    {
        _entregadorRepository = entregadorRepository;
    }

    public async Task<Entregador> CreateEntregadorAsync(Entregador entregador)
    {
        // Check if an entregador with the same CNPJ already exists
        if (await _entregadorRepository.EntregadorExistsByCnpjAsync(entregador.Cnpj))
        {
            throw new ArgumentException("Entregado not exits");
        }

        // Check if an entregador with the same CNH number already exists
        if (await _entregadorRepository.EntregadorExistsByNumeroCnhAsync(entregador.NumeroCnh))
        {
            throw new ArgumentException("Entregador not exists");
        }

       

        return await _entregadorRepository.CreateEntregadorAsync(entregador);
    }
}