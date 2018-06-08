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
        public static readonly IList<Money> acceptableCoins = new List<Money>() {
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
        /// <param name="count">How many products do we want to add.</param>
        /// <param name="productNumber">Product number in candy machine product list.</param>
        public void AddProduct(Product product, int count, int productNumber)
        {
            ValidateProduct(product, count, productNumber);

            shelves[productNumber].AddProduct(product, count);
        }

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

            // Don't check anything if shelf is empty and user is trying to add the first product.
            if (shelf.Product != null)
            {
                // Check if user is trying to add different product to productNumber position
                if (!shelf.Product.Equals(product))
                {
                    throw new ArgumentException("You cannot add different products in one shelf");
                }
            }
        }

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

        public int GetProductAmount(int productNumber)
        {
            return shelves[productNumber].ProductCount;
        }

        public Money InsertCoin(Money amount)
        {
            if (amount.Euros <= 0 && amount.Cents <= 0)
            {
                throw new ArgumentOutOfRangeException("You must insert at least 10 cents");
            }

            // Validate inserted coin
            if (false == IsCoinValid(amount))
            {
                throw new ArgumentOutOfRangeException($"Candy machine accepts only {MoneyHelper.ConvertMoneyListToString(acceptableCoins)} coins");
            }

            Amount = MoneyHelper.AddMoney(Amount, amount);

            return Amount;
        }

        public Money ReturnMoney()
        {
            Money returnMoney = Amount;
            Amount = new Money();

            return returnMoney;
        }

        /// <summary>Checks if shelf contains given product.</summary>
        ///<param name="productNumber">Product number in candy machine product list.</param>
        ///<param name="product">Product that we want to find.</param>
        public bool HasProduct(int productNumber, Product product)
        {
            return shelves[productNumber].ContainsProduct(product);
        }

        public static bool IsCoinValid(Money coin)
        {
            return acceptableCoins.Contains(coin);
        }
    }
}
