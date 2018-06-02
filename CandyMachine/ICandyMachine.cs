using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyMachine
{
    public interface ICandyMachine
    {
        ///<summary>Candy machine manufacturer.</summary>
        string Manufacturer { get; }

        ///<summary>Amount of money inserted into candy machine.</summary>
        Money Amount { get; }

        ///<summary>Calculates remaining amount of product</summary>
        int GetProductAmount(int productNumber);

        ///<summary>
        /// Adds given amount of products to the given product number position
        /// </summary>
        void AddProduct(Product product, int count, int productNumber);

        ///<summary>Inserts the coin into candy machine.</summary>
        ///<paramname="amount">Coin nominal value.</param>
        Money InsertCoin(Money amount);

        ///<summary>
        /// Returns all inserted coins back to user if purchase did not
        /// happened or returns remainder.
        /// </summary>
        Money ReturnMoney();

        ///<summary>Buys product from list of products.</summary>
        ///<paramname="productNumber">Product number in candy machine product list.</param>
        Product Buy(int productNumber);
    }

    public struct Money
    {
        public int Euros { get; set; }
        public int Cents { get; set; }
    }

    public struct Product
    {
        ///<summary>Gets or sets the product price.</summary>
        public Money Price { get; set; }

        ///<summary>Gets or sets the product name.</summary>
        public string Name { get; set; }
    }

}
