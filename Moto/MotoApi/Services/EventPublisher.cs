using Confluent.Kafka;
using MotoApi.Models;
using MotoApi.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MotoApi.Configuration;
using System.Text.Json;

namespace MotoApi.Services
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ILogger<EventPublisher> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly KafkaConfiguration _kafkaConfig;

        public EventPublisher(ILogger<EventPublisher> logger, 
                             IServiceProvider serviceProvider, 
                             IOptions<KafkaConfiguration> kafkaConfig)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _kafkaConfig = kafkaConfig.Value;
        }

        public async Task PublishMotoCadastradaAsync(MotoCadastradaEvent evento)
        {
            _logger.LogInformation("Publicando evento de moto cadastrada: {Identificador} para o tópico {Topic}", 
                                  evento.Identificador, _kafkaConfig.TopicName);
            
            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaConfig.BootstrapServers
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                // Serializar o evento para JSON
                var eventoJson = JsonSerializer.Serialize(evento);
                
                // Enviar mensagem para o Kafka
                var deliveryResult = await producer.ProduceAsync(_kafkaConfig.TopicName, 
                    new Message<Null, string> { Value = eventoJson });

                _logger.LogInformation($"Evento de moto cadastrada enviado com sucesso para o Kafka: {deliveryResult.Message.Value}");
                
                // Agora vamos criar um consumidor para processar imediatamente a mensagem
                await ProcessMessageLocally(evento);
                
                // Aguardar até que todas as mensagens pendentes sejam entregues
                producer.Flush(TimeSpan.FromSeconds(10));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao publicar evento de moto cadastrada no Kafka para moto {Identificador}", 
                                evento.Identificador);
                throw;
            }
        }

        private async Task ProcessMessageLocally(MotoCadastradaEvent evento)
        {
            // Obter o consumidor de eventos e processar imediatamente
            using var scope = _serviceProvider.CreateScope();
            var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
            await eventConsumer.ProcessMotoCadastradaEventAsync(evento);
        }
    }
}