using MotoApi.Models;

namespace MotoApi.Services.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishMotoCadastradaAsync(MotoCadastradaEvent evento);
    }
}