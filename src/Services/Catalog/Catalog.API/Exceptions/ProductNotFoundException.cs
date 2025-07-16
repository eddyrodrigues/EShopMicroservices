namespace Catalog.API.Exceptions
{
    public class ProductNotFoundException : ArgumentNullException
    {
        public ProductNotFoundException(Guid productId)
        {
        }
    }
}
