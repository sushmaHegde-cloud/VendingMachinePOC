using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachinePOC.Interfaces;

namespace VendingMachinePOC
{
    public interface ICoinSpecification
    {
        decimal Diameter { get; set; }
        decimal Weight { get; set; }
        decimal Thickness { get; set; }        
    }
}
