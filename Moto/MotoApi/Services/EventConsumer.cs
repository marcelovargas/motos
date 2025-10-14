using MotoApi.Data;
using MotoApi.Models;
using MotoApi.Repositories.Interfaces;
using MotoApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MotoApi.Services
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EventConsumer> _logger;

        public EventConsumer(ApplicationDbContext context, ILogger<EventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ProcessMotoCadastradaEventAsync(MotoCadastradaEvent evento)
        {
            _logger.LogInformation("Processando evento de moto cadastrada: {Identificador} ano {Ano}", evento.Identificador, evento.Ano);
            
            
            if (evento.Ano == 2024)
            {
                _logger.LogInformation("Moto cadastrada é do ano 2024. Armazenando no banco de dados.");

               
                var eventoMoto2024 = new EventoMoto2024
                {
                    Identificador = evento.Identificador,
                    Ano = evento.Ano,
                    Modelo = evento.Modelo,
                    Placa = evento.Placa,
                    DataRegistroEvento = evento.DataRegistro,
                    DataRecebimento = DateTime.UtcNow
                };

                _context.EventosMoto2024.Add(eventoMoto2024);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Evento de moto 2024 armazenado com sucesso no banco de dados.");
            }
            else
            {
                _logger.LogInformation("Moto cadastrada não é do ano 2024. Ignorando.");
            }
            
            await Task.CompletedTask; 
        }
    }
}