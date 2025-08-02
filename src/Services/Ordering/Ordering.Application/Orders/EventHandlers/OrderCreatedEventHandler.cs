
namespace Ordering.Application.Orders.EventHandlers
{
    public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
        : INotificationHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {

            logger.LogInformation(
                "Order with ID {OrderId} has been created. Customer ID: {CustomerId}, Total Price: {TotalPrice}",
                notification.order.Id, notification.order.CustomerId, notification.order.TotalPrice);
            return Task.CompletedTask;
        }
    }
}
