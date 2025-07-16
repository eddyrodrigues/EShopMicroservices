
namespace Catalog.API.Products.GetProductById
{

    public record GetProductByIdQueryResult
    {
        public GetProductByIdQueryResult(
            Guid id,
            string name,
            List<string> category,
            string description,
            string imageFile,
            decimal price)
        {
            Id = id;
            Name = name;
            Category = category;
            Description = description;
            ImageFile = imageFile;
            Price = price;
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> Category { get; set; } = new List<string>();
        public string Description { get; set; } = default!;
        public string ImageFile { get; set; } = default!;
        public decimal Price { get; set; }
    }

    public record GetProductByIdQueryRequest(Guid Id) : IQuery<GetProductByIdQueryResult>
    {

    }


    public class GetProductByIdQueryHandler(IDocumentSession session): IQueryHandler<GetProductByIdQueryRequest, GetProductByIdQueryResult>
    {
        public async Task<GetProductByIdQueryResult> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var response = await session.LoadAsync<Product>(request.Id, cancellationToken);

            return response != null ? response.Adapt<GetProductByIdQueryResult>() :
                throw new ProductNotFoundException(request.Id);

        }
    }
}
