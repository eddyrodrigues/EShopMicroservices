using Microsoft.EntityFrameworkCore;

namespace Ordering.Application.Orders.Queries.GetOrderByName
{

    public record GetOrdersByNameQuery(string OrderName) : IQuery<GetOrdersByNameResult>;
    public class GetOrdersByNameResult
    {
        public IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }

    public class GetOrderByNameHandler (IApplicationDbContext dbContext)
        : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
        {

            var orders = await dbContext.Orders.Include(c => c.OrderItems)
                .AsNoTracking()
                .Where(c => c.OrderName.Value.Equals(query.OrderName))
                .OrderBy(c => c.OrderName)
                .ToListAsync(cancellationToken);


            var orderDtoResult = orders.ToOrderDtoList();


            return new GetOrdersByNameResult { Orders = orderDtoResult };
        }
    }
}
