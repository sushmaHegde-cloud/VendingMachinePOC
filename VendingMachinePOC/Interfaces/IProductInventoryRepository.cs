using System.Collections.Generic;

namespace VendingMachinePOC
{
    public interface IProductInventoryRepository
    {
        Dictionary<string, int> GetInventory();
        void UpdateInventory(string code);
    }
}