using Confluent.Kafka;
using MotoApi.Models;
using MotoApi.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MotoApi.Configuration;
using System.Text.Json;

namespace MotoApi.Services
{
    public class KafkaEventConsumerService : BackgroundService
    {
        private readonly ILogger<KafkaEventConsumerService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly KafkaConfiguration _kafkaConfig;

        public KafkaEventConsumerService(ILogger<KafkaEventConsumerService> logger,
                                       IServiceProvider serviceProvider,
                                       IOptions<KafkaConfiguration> kafkaConfig)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _kafkaConfig = kafkaConfig.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaConfig.BootstrapServers,
                GroupId = _kafkaConfig.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_kafkaConfig.TopicName);

            _logger.LogInformation("Iniciando o consumidor Kafka para o tópico: {Topic}", _kafkaConfig.TopicName);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);
                        
                        if (consumeResult?.Message?.Value != null)
                        {
                            _logger.LogInformation("Mensagem recebida do Kafka: {Message}", consumeResult.Message.Value);

                            // Desserializar o evento
                            var evento = JsonSerializer.Deserialize<MotoCadastradaEvent>(consumeResult.Message.Value);
                            
                            if (evento != null)
                            {
                                // Processar o evento usando o serviço de consumo
                                using var scope = _serviceProvider.CreateScope();
                                var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
                                await eventConsumer.ProcessMotoCadastradaEventAsync(evento);
                                
                                _logger.LogInformation("Evento de moto processado com sucesso: {Identificador}", evento.Identificador);
                            }
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Erro de consume no Kafka: {ErrorMessage}", ex.Error.Reason);
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex, "Erro ao desserializar mensagem do Kafka");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro inesperado ao processar mensagem do Kafka");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Operação cancelada. Encerrando o consumidor Kafka.");
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}