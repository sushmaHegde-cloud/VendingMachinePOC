using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachinePOC.Interfaces;

namespace VendingMachinePOC
{
    public class ValidCoin : ICoinSpecification
    {
        public decimal Diameter { get; set; }
        public decimal Weight { get; set; }
        public decimal Thickness { get; set; }
        public CoinType Type { get; set; }
        public decimal Value { get; set; }
    }
}
