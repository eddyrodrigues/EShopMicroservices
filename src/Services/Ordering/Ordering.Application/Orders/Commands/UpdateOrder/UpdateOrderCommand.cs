namespace Ordering.Application.Orders.Commands.UpdateOrder
{

    public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult> { }

    public record UpdateOrderResult
    {
        public bool IsSuccess { get; set; }
    }

    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Order ID is required.");
            RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Order name is required.");
            RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
        }
    }

    public class UpdateOrderCommandHandler 
        : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        private readonly IApplicationDbContext _context;
        public UpdateOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.Order.Id);
            var order = await _context.Orders.FindAsync(orderId, cancellationToken);
            if (order == null)
            {
                throw new OrderNotFoundException(command.Order.Id);
            }

            UpdateOrderWithNewValues(order, command.Order);
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(cancellationToken);
            
            return new UpdateOrderResult { IsSuccess = true };
        }

        private void UpdateOrderWithNewValues(Order order, OrderDto orderCommand)
        {
            var shippingAddress = Address.Of(
               orderCommand.ShippingAddress.FirstName,
               orderCommand.ShippingAddress.LastName,
               orderCommand.ShippingAddress.EmailAddress,
               orderCommand.ShippingAddress.AddressLine,
               orderCommand.ShippingAddress.Country,
               orderCommand.ShippingAddress.State,
               orderCommand.ShippingAddress.ZipCode);
            var billingAddress = Address.Of(
                orderCommand.BillingAddress.FirstName,
                orderCommand.BillingAddress.LastName,
                orderCommand.BillingAddress.EmailAddress,
                orderCommand.BillingAddress.AddressLine,
                orderCommand.BillingAddress.Country,
                orderCommand.BillingAddress.State,
                orderCommand.BillingAddress.ZipCode);

            var paymentMethod = Payment.Of(
                orderCommand.Payment.CardName,
                orderCommand.Payment.CardNumber,
                orderCommand.Payment.Expiration,
                orderCommand.Payment.Cvv,
                orderCommand.Payment.PaymentMethod);

            order.Update(OrderName.Of(orderCommand.OrderName), shippingAddress, billingAddress, paymentMethod, orderCommand.Status);
        }
    }

}
