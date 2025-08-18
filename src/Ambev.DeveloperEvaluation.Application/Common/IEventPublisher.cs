using MediatR;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Common
{
    public interface IEventPublisher
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : INotification;
    }
}

