using Marten.Schema;

namespace Basket.API.Models
{
    public class ShoppingCart
    {
        [Identity]
        public string UserName { get; set; } = default!;
        public List<ShoppingCartItem> Items { get; set; } = [];
        public decimal TotalPrice => this.Items.Sum(x => x.Price);
        
        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public ShoppingCart()
        {
            
        }
    }

    public class ShoppingCartItem
    {
        public int Quantity { get; set; } = default!;
        public string Color { get; set; } = default!;
        public string ProductName { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public Guid ProductId { get; set; } = default!;
        public string Description { get; set; } = default!;

    }
}
