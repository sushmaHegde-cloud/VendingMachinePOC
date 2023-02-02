using System.Collections.Generic;

namespace VendingMachinePOC
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProductList();
    }
}