using Mapster;

namespace Basket.API.Basket.GetBasket
{
    public record GetBasketRquest(string UserName);
    public record GetBasketResponse(ShoppingCart Cart);
    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userId}", async (string userId, ISender sender) =>
            {
                var query = (new GetBasketRquest(userId)).Adapt<GetBasketQuery>();
                var result = await sender.Send(query);
                var basket = result.Adapt<GetBasketResponse>();
                return Results.Ok(basket);
                //return result.Match(
                //    error => Results.Problem(error.Message));
            })
            .WithName("GetBasket")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get User Basket")
            .WithDescription("Retrieve the basket for a specific user by user ID.");
        }
    }
}
