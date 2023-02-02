using System.Collections;
using System.Collections.Generic;

namespace VendingMachinePOC
{
    public class ProductRepository : IProductRepository
    {
        private static List<Product> products;
        
        public IEnumerable<Product> GetProductList()
        {
            return products ?? (products = new List<Product>
            {
                new Product() {Code = "COLA1", Type = ProductItemType.Cola, Name = "cola", Price = 1.00m},
                new Product() {Code = "CHIPS1", Type = ProductItemType.Chips, Name = "chips", Price = 1.00m},
                new Product() {Code = "CANDY1", Type = ProductItemType.Candy, Name = "candy", Price = 0.75m}
            });
        }
    }
}