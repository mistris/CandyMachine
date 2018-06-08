using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyMachine
{
    public struct Product
    {
        ///<summary>Gets or sets the product price.</summary>
        public Money Price { get; set; }

        ///<summary>Gets or sets the product name.</summary>
        public string Name { get; set; }
    }
}
