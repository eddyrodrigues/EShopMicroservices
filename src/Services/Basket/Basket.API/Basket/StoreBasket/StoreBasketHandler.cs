
using Basket.API.Data;
using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket
{

    public record StoreBasketCommand(ShoppingCart Cart): ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketRequest>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null.");
            RuleFor(x => x.Cart.Items).NotEmpty().WithMessage("Cart must contain at least one item.");
        }
    }

    public class StoreBasketCommandHandler(IBasketRepository basketRepository, DiscountProtoService.DiscountProtoServiceClient discountProtoService) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
        {
            await DeductDiscount(request, cancellationToken);

            ShoppingCart cart = request.Cart;
            var basket = await basketRepository.StoreBasket(cart);

            return new StoreBasketResult(basket.UserName);
        }

        private async Task DeductDiscount(StoreBasketCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.Cart.Items)
            {
                var coupon = await discountProtoService.GetDiscountAsync(new GetDiscountRequest()
                {
                    ProductName = item.ProductName,
                }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }
        }
    }
}
