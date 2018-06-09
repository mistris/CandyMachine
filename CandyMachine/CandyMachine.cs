using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ValidateProduct(product, count, productNumber);

            shelves[productNumber].AddProduct(product, count);
        }

        /// <summary>
        /// Checks if product number is valid.
        /// Checks if there is enough space in specified candy machine position (shelf number).
        /// Checks if user is trying to add different products in one shelf.
        /// Checks if product price can be made of acceptable coins.
        /// </summary>
        /// <param name="product">Product that we want to add.</param>
        /// <param name="count">Units of product.</param>
        /// <param name="productNumber">Product number in candy machine product list (shelf number).</param>
        private void ValidateProduct(Product product, int count, int productNumber)
        {
            // Check if productNumber position is valid
            if (productNumber < 0 || productNumber > shelves.Length - 1)
            {
                throw new IndexOutOfRangeException($"productNumber must be >= 0 and <= {shelves.Length - 1}");
            }

            Shelf shelf = shelves[productNumber];

            // Check if there is enough space for product in specified productNumber position
            int alreadyAddedProductCount = shelf == null ? 0 : shelf.ProductCount;
            int spaceLeftInShelf = shelf.ShelfSize - alreadyAddedProductCount;

            if (spaceLeftInShelf == 0)
            {
                throw new Exception("Shelf is full. You cannot add more products to it.");
            }
            else
            {
                if (count < 1 || count > spaceLeftInShelf)
                {
                    throw new ArgumentException($"Product count must be > 0 and <= {spaceLeftInShelf}");
                }
            }

            // Check if user is trying to add different product to productNumber position
            if (shelf.Product != null)
            {
                if (!shelf.Product.Equals(product))
                {
                    throw new ArgumentException("You cannot add different products in one shelf");
                }
            }

            // Check if product price is correct (it can be made of acceptable coins)
            if (false == MoneyHelper.CanPriceBeMadeOfAcceptableCoins(product.Price, acceptableCoins))
            {
                throw new ArgumentException($"Price {MoneyHelper.ConvertMoneyToString(product.Price)} cannot be made of acceptable coins: {MoneyHelper.ConvertMoneyListToString(acceptableCoins)}");
            }
        }

        ///<summary>Buys product from list of products.</summary>
        ///<param name="productNumber">Product number in candy machine product list.</param>
        ///<returns>Returns instance of Product that we bought.</returns>
        public Product Buy(int productNumber)
        {
            Shelf shelf = shelves[productNumber];

            // Do not allow purchase if shelf doesn't contain at least one unit of the selected product.
            if (shelf.ProductCount < 1)
            {
                throw new Exception("The selected product is out of stock");
            }

            // Decrease selected product count and subtract price from current amount of money.
            Product selectedProduct = shelf.Product.Value;
            shelf.DecreaseProductCount();

            Amount = MoneyHelper.SubtractMoney(Amount, selectedProduct.Price);

            return selectedProduct;
        }

        ///<summary>Calculates remaining amount of product</summary>
        public int GetProductAmount(int productNumber)
        {
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
                throw new ArgumentOutOfRangeException($"Candy machine accepts only {MoneyHelper.ConvertMoneyListToString(acceptableCoins)} coins");
            }

            Amount = MoneyHelper.AddMoney(Amount, amount);

            return Amount;
        }

        /// <summary>Returns current amount of money (remainder)</summary>
        public Money ReturnMoney()
        {
            Money returnMoney = Amount;
            Amount = new Money();

            return returnMoney;
        }

        ///<summary>Checks if candy machine contains given product in the specified shelf.</summary>
        ///<param name="productNumber">Product number in candy machine product list (shelf number).</param>
        ///<param name="product">Product that we want to find.</param>
        public bool HasProduct(int productNumber, Product product)
        {
            return shelves[productNumber].ContainsProduct(product);
        }
        
        /// <summary>Checks if given coin is valid.</summary>
        /// <param name="coin">Instance of Money that we want to validate.</param>
        /// <returns>Returns true if given coin is valid.</returns>
        public static bool IsCoinValid(Money coin)
        {
            if (coin.Euros <= 0 && coin.Cents <= 0)
            {
                // TODO: get smallest coin nominal value
                throw new ArgumentOutOfRangeException("You must insert at least 10 cents");
            }

            return acceptableCoins.Contains(coin);
        }
    }
}
