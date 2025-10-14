namespace MotoApi.Configuration
{
    public class KafkaConfiguration
    {
        public string BootstrapServers { get; set; } = "localhost:9092";
        public string TopicName { get; set; } = "moto-cadastrada";
        public string GroupId { get; set; } = "moto-api-group";
    }
}