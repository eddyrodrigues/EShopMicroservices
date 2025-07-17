
using Marten.Pagination;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQueryResult(IEnumerable<Product> Products)
    {
        
    }
    public record GetProductsQueryRequest(): IQuery<GetProductsQueryResult>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetProductsQueryHandler : IQueryHandler<GetProductsQueryRequest, GetProductsQueryResult>
    {
        private readonly IDocumentSession session;
        private readonly ILogger<GetProductsQueryHandler> logger;

        public GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
        {
            this.session = session;
            this.logger = logger;
        }
        public async Task<GetProductsQueryResult> Handle(GetProductsQueryRequest request, CancellationToken cancellationToken)
        {

            var result = await this.session.Query<Product>().ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);


            return new GetProductsQueryResult(result);
        }
    }
}
