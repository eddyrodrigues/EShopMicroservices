
namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQueryResult(IEnumerable<Product> Products)
    {
        
    }
    public record GetProductsQueryRequest(): IQuery<GetProductsQueryResult>
    {

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

            var result = await this.session.Query<Product>().ToListAsync(cancellationToken);


            return new GetProductsQueryResult(result);
        }
    }
}
