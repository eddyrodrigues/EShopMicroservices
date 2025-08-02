

namespace Ordering.Application.Orders.EventHandlers
{
    public class OrderUpdateEventHandler (ILogger<OrderUpdateEventHandler> logger)
        : INotificationHandler<OrderUpdatedEvent>
    {
        public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Order with ID {OrderId} has been updated. New status: {Status}",
                notification.order.Id, notification.order.Status);

            return Task.CompletedTask;
        }
    }
}
