
namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQueryResponse
    {
        public GetProductByIdQueryResponse(
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
    public class GetProductByIdEndpoint() : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{productId:Guid}", async (Guid productId, ISender sender) =>
            {
                var request = new GetProductByIdQueryRequest(productId);

                var result = await sender.Send<GetProductByIdQueryResult>(request);

                var response = result.Adapt<GetProductByIdQueryResponse>();
                return Results.Ok(response);
            });
        }
    }
}
