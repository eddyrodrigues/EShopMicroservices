﻿
using Basket.API.Data;

namespace Basket.API.Basket.GetBasket
{

    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

    public record GetBasketResult(ShoppingCart Cart);

    public class GetBasketQueryHandler(IBasketRepository basketRepository) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {

            // get from db
            //var cart = await _repository.GetCartAsync(request.UserName, cancellationToken);

            var basket = await basketRepository.GetBasket(request.UserName);

            return new GetBasketResult(basket);
        }
    }
}
