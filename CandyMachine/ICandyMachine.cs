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
        ///<param name="amount">Coin nominal value.</param>
        Money InsertCoin(Money amount);

        ///<summary>
        /// Returns all inserted coins back to user if purchase did not
        /// happened or returns remainder.
        /// </summary>
        Money ReturnMoney();

        ///<summary>Buys product from list of products.</summary>
        ///<param name="productNumber">Product number in candy machine product list.</param>
        Product Buy(int productNumber);
    }
}
