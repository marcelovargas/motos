using MotoApi.Models;

namespace MotoApi.Services.Interfaces
{
    public interface IEventConsumer
    {
        Task ProcessMotoCadastradaEventAsync(MotoCadastradaEvent evento);
    }
}