using MotoApi.Models;
using MotoApi.Repositories.Interfaces;
using MotoApi.Services.Interfaces;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace MotoApi.Services;

public class MotoService : IMotoService
{
    private readonly IMotoRepository _motoRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<MotoService> _logger;

    public MotoService(IMotoRepository motoRepository, IEventPublisher eventPublisher, ILogger<MotoService> logger)
    {
        _motoRepository = motoRepository;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<Moto> CreateMotoAsync(Moto moto)
    {
        // Check if a moto with the same plate already exists
        if (await _motoRepository.MotoExistsByPlacaAsync(moto.Placa))
        {
            throw new ArgumentException($"A moto with plate {moto.Placa} already exists.");
        }

        var createdMoto = await _motoRepository.CreateMotoAsync(moto);
        
        // Publicar evento de moto cadastrada
        try
        {
            var evento = new MotoCadastradaEvent
            {
                Identificador = createdMoto.Identificador,
                Ano = createdMoto.Ano,
                Modelo = createdMoto.Modelo,
                Placa = createdMoto.Placa,
                DataRegistro = DateTime.UtcNow
            };

            await _eventPublisher.PublishMotoCadastradaAsync(evento);
            _logger.LogInformation("Evento de moto cadastrada publicado para moto {Identificador}", createdMoto.Identificador);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar evento de moto cadastrada para moto {Identificador}", createdMoto.Identificador);
            // Não lançar exceção aqui para não comprometer o cadastro da moto
        }

        return createdMoto;
    }

    public async Task<Moto?> GetMotoByIdAsync(string id)
    {
        return await _motoRepository.GetMotoByIdAsync(id);
    }

    public async Task<IEnumerable<Moto>> GetMotosAsync(string? placa = null)
    {
        Expression<Func<Moto, bool>>? filter = null;

        if (!string.IsNullOrEmpty(placa))
        {
            filter = m => m.Placa == placa;
        }

        return await _motoRepository.GetMotosAsync(filter);
    }

    public async Task<bool> UpdateMotoPlacaAsync(string id, string placa)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(placa))
        {
            throw new ArgumentException("Dados inválidos");
        }

        
        var regex = new System.Text.RegularExpressions.Regex(@"^[A-Z]{3}-\d{4}$|^[A-Z]{3}-\d[A-Z]\d{2}$");
        if (!regex.IsMatch(placa))
        {
            throw new ArgumentException("Dados inválidos");
        }

        // Check if another moto already has this plate
        if (await _motoRepository.MotoExistsByPlacaAsync(placa))
        {
            throw new ArgumentException("Dados inválidos");
        }

        return await _motoRepository.UpdateMotoPlacaAsync(id, placa);
    }

    public async Task<bool> DeleteMotoAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException("Invalid identifier");
        }


        var moto = await _motoRepository.GetMotoByIdAsync(id);
        if (moto == null)
        {
            throw new ArgumentException("Moto não encontrada");
        }


        if (moto.Locacoes != null && moto.Locacoes.Any())
        {
            throw new InvalidOperationException("Não é possível remover uma moto que possui locações registradas");
        }

        return await _motoRepository.DeleteMotoAsync(id);
    }
}