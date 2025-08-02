using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public record DeleteOrderCommand(Guid OrderId) : ICommand<DeleteOrderResult> { }
    public record DeleteOrderResult
    {
        public bool IsSuccess { get; set; }
    }

    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order ID is required.");
        }
    }

    public class DeleteOrderCommandHandler 
        : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        private readonly IApplicationDbContext _context;
        public DeleteOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.OrderId);
            var order = await _context.Orders.FindAsync(orderId, cancellationToken);
            if (order == null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteOrderResult { IsSuccess = true };
        }
    }
}
