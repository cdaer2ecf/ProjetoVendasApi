using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Common
{
    public class LoggingEventPublisher : IEventPublisher
    {
        private readonly ILogger<LoggingEventPublisher> _logger;

        public LoggingEventPublisher(ILogger<LoggingEventPublisher> logger)
        {
            _logger = logger;
        }

        public Task Publish<TEvent>(TEvent @event) where TEvent : INotification
        {
            _logger.LogInformation("Event Published: {EventType} - Data: {@EventData}", @event.GetType().Name, @event);
            return Task.CompletedTask;
        }
    }
}

