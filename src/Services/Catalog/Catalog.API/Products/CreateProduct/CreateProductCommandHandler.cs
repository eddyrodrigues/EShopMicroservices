using FluentValidation;

namespace Catalog.API.Products.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IDocumentSession _session;

        public CreateProductCommandHandler(IDocumentSession session)
        {
            this._session = session;
        }
        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // create product entity
            var product = new Product
            {
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Price = request.Price,
                Id = Guid.NewGuid()
            };
            // save to db
            _session.Store(product);
            await _session.SaveChangesAsync(cancellationToken);

            // return CreateProductResult
            return new CreateProductResult(product.Id);
        }
    }

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price is required");
        }
    }

    public record CreateProductCommand : ICommand<CreateProductResult>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> Category { get; set; } = new List<string>();
        public string Description { get; set; } = default!;
        public string ImageFile { get; set; } = default!;
        public decimal Price { get; set; }
    }
    public record CreateProductResult(Guid Id);

}
