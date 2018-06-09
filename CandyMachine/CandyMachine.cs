using CandyMachine.CandyMachineExceptions;
using System.Collections.Generic;
using System.Linq;

namespace CandyMachine
{
    public class CandyMachine : ICandyMachine
    {
        public string Manufacturer { get; }

        public Money Amount { get; private set; }

        /// <summary>
        /// Contains number of shelves in candy machine.
        /// Shelf is a horizontal section in a candy machine where products are stored.
        /// </summary>
        public int ShelfCount { get; private set; }

        /// <summary>Candy machine shelves which contain products.</summary>
        private readonly Shelf[] shelves;

        /// <summary>Coins which candy machine accepts.</summary>
        private static readonly IList<Money> acceptableCoins = new List<Money>() {
            new Money { Euros = 0, Cents = 10 },
            new Money { Euros = 0, Cents = 20 },
            new Money { Euros = 0, Cents = 50 },
            new Money { Euros = 1, Cents = 0 }
        };

        public CandyMachine(string manufacturer, int shelfCount, int shelfSize)
        {
            Manufacturer = manufacturer;
            ShelfCount = shelfCount;

            // Add empty shelves to candy machine
            shelves = new Shelf[shelfCount].Select(s => new Shelf(shelfSize)).ToArray();
        }

        /// <summary>Adds a product to the candy machine.</summary>
        /// <param name="product">Product that is sold.</param>
        /// <param name="count">How many units of product do we want to add.</param>
        /// <param name="productNumber">Product number in candy machine product list.</param>
        public void AddProduct(Product product, int count, int productNumber)
        {
            ValidateProductNumber(productNumber);

            // Check if product price is correct (it can be made of acceptable coins)
            if (false == MoneyHelper.CanPriceBeMadeOfAcceptableCoins(product.Price, acceptableCoins))
            {
                throw new InvalidPriceException($"Price {MoneyHelper.ConvertMoneyToString(product.Price)} cannot be made of acceptable coins: {MoneyHelper.ConvertMoneyListToString(acceptableCoins)}");
            }

            shelves[productNumber].AddProduct(product, count);
        }

        ///<summary>Buys product from list of products.</summary>
        ///<param name="productNumber">Product number in candy machine product list.</param>
        ///<returns>Returns instance of Product that we bought.</returns>
        public Product Buy(int productNumber)
        {
            ValidateProductNumber(productNumber);

            Shelf shelf = shelves[productNumber];

            // Do not allow purchase if shelf doesn't contain at least one unit of the selected product.
            if (shelf.IsEmpty())
            {
                throw new ProductOutOfStockException("The selected product is out of stock");
            }

            Product selectedProduct = shelf.Product.Value;

            // Check if there is enough current amount of money to buy selected product
            if (MoneyHelper.ConvertMoneyToCents(Amount) < MoneyHelper.ConvertMoneyToCents(selectedProduct.Price))
            {
                throw new NotEnoughMoneyException("Selected product's price is greater than current amount of money.");
            }

            // Decrease selected product count and subtract price from current amount of money.
            shelf.DecreaseProductCount();
            Amount = MoneyHelper.SubtractMoney(Amount, selectedProduct.Price);

            return selectedProduct;
        }

        ///<summary>Calculates remaining amount of product</summary>
        public int GetProductAmount(int productNumber)
        {
            ValidateProductNumber(productNumber);

            return shelves[productNumber].ProductCount;
        }

        ///<summary>Inserts the coin into candy machine.</summary>
        ///<param name="amount">Coin nominal value.</param>
        ///<returns>Returns current amount of money.</returns>
        public Money InsertCoin(Money amount)
        {
            // Validate inserted coin
            if (false == IsCoinValid(amount))
            {
                throw new InvalidCoinException($"Candy machine accepts only {MoneyHelper.ConvertMoneyListToString(acceptableCoins)} coins");
            }

            Amount = MoneyHelper.AddMoney(Amount, amount);

            return Amount;
        }

        /// <summary>Returns current amount of money (remainder)</summary>
        public Money ReturnMoney()
        {
            // Put remainder to return in a separate variable in order to empty current amount of money.
            Money returnMoney = Amount;
            Amount = new Money();

            return returnMoney;
        }

        ///<summary>Checks if candy machine contains given product in the specified shelf.</summary>
        ///<param name="productNumber">Product number in candy machine product list (shelf number).</param>
        ///<param name="product">Product that we want to find.</param>
        public bool HasProduct(int productNumber, Product product)
        {
            ValidateProductNumber(productNumber);

            return shelves[productNumber].ContainsProduct(product);
        }

        /// <summary>Checks if given coin is valid.</summary>
        /// <param name="coin">Instance of Money that we want to validate.</param>
        /// <returns>Returns true if given coin is valid.</returns>
        public static bool IsCoinValid(Money coin)
        {
            return acceptableCoins.Contains(coin);
        }
        
        /// <summary>Checks if shelf with given number exists.</summary>
        /// <param name="productNumber">Product number which represents shelf number.</param>
        /// <returns>Returns true if candy machine contains shelf with given number.</returns>
        private void ValidateProductNumber(int productNumber)
        {
            if (productNumber < 0 || productNumber > ShelfCount - 1)
            {
                throw new InvalidProductNumberException($"Product number must be between 0 and {ShelfCount - 1}");
            }
        }
    }
}
