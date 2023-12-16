using MediatR;

namespace CoolForecast.Api.Core.Notifications;

internal sealed class ForecastCreatedEvent : INotification
{
    public required Guid ForecastId { get; init; }
}