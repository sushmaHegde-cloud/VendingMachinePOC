using System.Collections.Generic;

namespace VendingMachinePOC
{
    public interface IVendingMachine
    {        
        VendingResponse AcceptCoin(InputCoin coin);
        VendingResponse SelectProduct(string code);
        IEnumerable<ItemChange> ReturnCoins();       
    }
}