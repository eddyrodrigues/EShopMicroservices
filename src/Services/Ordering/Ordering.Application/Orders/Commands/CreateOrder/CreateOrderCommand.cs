

using Ordering.Application.Data;


namespace Ordering.Application.Orders.Commands.CreateOrder
{

    public record CreateOrderResult
    {
        public Guid Id { get; set; }
    }
    public record CreateOrderCommand(OrderDto order) : ICommand<CreateOrderResult> { }

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.order.OrderName).NotEmpty()
                .WithMessage("Order name is required.");
            RuleFor(x => x.order.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
            RuleFor(x => x.order.OrderItems).NotEmpty()
                .WithMessage("OrderItems is required.");
        }
    }

    public class CreateOrderCommandHandler 
        : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        private readonly IApplicationDbContext _context;
        public CreateOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            Order order = CreateNewOrder(command.order);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateOrderResult { Id = order.Id.Value };
        }

        private Order CreateNewOrder(OrderDto order)
        {
            var shippingAddress = Address.Of(order.ShippingAddress.FirstName,
                order.ShippingAddress.LastName,
                order.ShippingAddress.EmailAddress,
                order.ShippingAddress.AddressLine,
                order.ShippingAddress.Country,
                order.ShippingAddress.State,
                order.ShippingAddress.ZipCode);
            var billingAddress = Address.Of(order.BillingAddress.FirstName,
                order.BillingAddress.LastName,
                order.BillingAddress.EmailAddress,
                order.BillingAddress.AddressLine,
                order.BillingAddress.Country,
                order.BillingAddress.State,
                order.BillingAddress.ZipCode);

            var newOrder = Order.Create(
                OrderId.Of(order.Id),
                CustomerId.Of(order.CustomerId),
                OrderName.Of(order.OrderName),
                shippingAddress,
                billingAddress,
                Payment.Of(order.Payment.CardName, order.Payment.CardNumber, order.Payment.Expiration, order.Payment.Cvv, order.Payment.PaymentMethod)
            );

            foreach (var item in order.OrderItems)
            {
                newOrder.Add(ProductId.Of(item.ProductId), item.Quantity, item.Price);
            }

            return newOrder;
        }
    }
}
