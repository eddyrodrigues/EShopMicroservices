
using Microsoft.EntityFrameworkCore;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    


    public record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<GetOrdersByCustomerResult>;
    public record GetOrdersByCustomerResult
    {
        public IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }


    public class GetOrdersByCustomerHandler(IApplicationDbContext dbContext)
        : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
    {
        public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
        {
            var orders = await dbContext.Orders.Include(c => c.OrderItems)
               .AsNoTracking()
               .Where(c => c.CustomerId == CustomerId.Of(request.CustomerId)))
               .OrderBy(c => c.OrderName)
               .ToListAsync(cancellationToken);


            var orderDtoResult = orders.ToOrderDtoList();


            return new GetOrdersByCustomerResult { Orders = orderDtoResult };
        }
    }
}
