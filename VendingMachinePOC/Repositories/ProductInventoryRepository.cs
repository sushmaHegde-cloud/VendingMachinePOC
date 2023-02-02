using System.Collections.Generic;

namespace VendingMachinePOC
{
    public class ProductInventoryRepository : IProductInventoryRepository
    {
        private static Dictionary<string, int> productQuantities;
        public Dictionary<string, int> GetInventory()
        {
            return productQuantities ?? (productQuantities = new Dictionary<string, int> {{ "COLA1", 2}, { "CHIPS1", 0}, { "CANDY1", 10}});
        }
        public void UpdateInventory(string code)
        {           
            var currentCount = productQuantities[code];
            if (currentCount > 0)
                productQuantities[code]--;
        }
    }
}