using Microsoft.AspNetCore.Cors.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachinePOC.Interfaces;

namespace VendingMachinePOC
{
    public class VendingMachinePOC : IVendingMachine
    {
        private readonly IProductService iproductService;
        private readonly ICoinService icoinService;
        private decimal cost;

        public VendingMachinePOC(ICoinService coinService, IProductService productService)
        {
            if (coinService == null) throw new ArgumentNullException("coinService parameter is null");
            if (productService == null) throw new ArgumentNullException("productService parameter is null");

            icoinService = coinService;
            iproductService = productService;
        }

        public VendingResponse AcceptCoin(InputCoin coin)
        {
            VendingResponse response = new VendingResponse();

            if (coin == null)
                throw new ArgumentNullException("Coin parameter null!");

            //check if the values correspond to an accepted coin            
            var currentCoin = icoinService.GetCoin(coin.Weight, coin.Diameter, coin.Thickness);

            //not a valid coin
            if (currentCoin == null)
            {
                response.Message = "Insert Coin";
                response.IsRejectedCoin = true;
                response.RejectedCoin = coin; //return rejected coin
                return response;
            }

            //valid coin
            cost += currentCoin.Value;
            response.Message = cost.ToString();
            response.IsSuccess = true;
            response.IsRejectedCoin = false;
            return response;
        }
        public VendingResponse SelectProduct(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException("Code parameter empty!");

            var response = new VendingResponse();

            //check if the code is valid            
            //if no, return error object with details
            var product = iproductService.GetProduct(code);

            //invalid code entered
            if (product == null)
            {
                response.Message = "Invalid Product Selected. Please try again";
                response.IsSuccess = false;
                return response;
            }

            //no coins entered, but selection pressed
            if (cost == 0)
            {
                //if exact change item, message = "exact change only"
                response.Message = "Insert Coin";
                response.IsSuccess = false;
                return response;
            }

            //entered coins less than cost
            if (cost < product.Price)
            {
                response.Message = string.Format("Price : {0}", product.Price);
                response.IsSuccess = false;
                return response;
            }

            //all good, valid code and valid amount entered
            var quantity = iproductService.GetProductQuantity(code);
            if (quantity > 0)
            {
                response.Message = "Thank You";
                response.IsSuccess = true;
                iproductService.UpdateProductQuantity(code);
                response.Change = MakeChange(Convert.ToDouble(cost - product.Price));
                cost = 0.00m;
                return response;
            }

            response.Message = "SOLD OUT";
            response.IsSuccess = false;
            return response;
        }
        public IEnumerable<ItemChange> ReturnCoins()
        {
            return MakeChange(Convert.ToDouble(cost));
        }

        private IEnumerable<ItemChange> MakeChange(double input)
        {
            List<ItemChange> itemchange = new List<ItemChange>();

            var coins = GetCoinValuesDictionary();

            var change = input;
            if (change == 0) return itemchange;

            foreach (var value in coins.Keys)
            {
                var result = (int)(change / coins[value]);
                if (result > 0)
                {
                    itemchange.Add(new ItemChange
                    {
                        Type = value,
                        Number = result
                    });

                    change = Math.Round(change - (result * coins[value]), 3);
                    var remainingAmount = change;
                    if (remainingAmount == 0)
                        return itemchange;
                }
            }
            return itemchange;
        }

        private Dictionary<CoinType, double> GetCoinValuesDictionary()
        {
            return new Dictionary<CoinType, double>
            {
                {CoinType.TwoPound, 2.00},
                {CoinType.OnePound, 1.00},
                {CoinType.FiftyPence, 0.50},
                {CoinType.TwentyPence, 0.20},
                {CoinType.TenPence, 0.10},
                {CoinType.FivePence, 0.05}
            };
        }
    }
}
