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

        /// <summary>Number of products you can store in one shelf.</summary>
        public int ShelfSize { get; private set; }

        /// <summary>Products that are sold (Product product, int count).</summary>
        private readonly Tuple<Product, int>[] products;

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
            ShelfSize = shelfSize;
            products = new Tuple<Product, int>[shelfCount];
        }

        /// <summary>Checks if candy machine contains given product.</summary>
        /// <param name="productNumber">Product number in candy machine product list.</param>
        /// <param name="product">Product that we want to find.</param>
        /// <returns></returns>
        public bool ContainsProduct(int productNumber, Product product)
        {
            return products[productNumber].Item1.Equals(product);
        }

        /// <summary>Adds a product to the candy machine.</summary>
        /// <param name="product">Product that is sold.</param>
        /// <param name="count">How many products do we want to add.</param>
        /// <param name="productNumber">Product number in candy machine product list.</param>
        public void AddProduct(Product product, int count, int productNumber)
        {
            ValidateProduct(product, count, productNumber);

            products[productNumber] = new Tuple<Product, int>(product, count);
        }

        private void ValidateProduct(Product product, int count, int productNumber)
        {
            // TODO: Check if productNumber position is valid
            if (productNumber < 0 || productNumber > products.Length - 1)
            {
                throw new IndexOutOfRangeException("productNumber must be >= 0 and <= ShelfCount");
            }

            Tuple<Product, int> productInShelf = products[productNumber];

            // TODO: Check if there is enough space for product in specified productNumber position
            int alreadyAddedProductCount = productInShelf == null ? 0 : productInShelf.Item2;

            if (count < 1 || count > ShelfSize - alreadyAddedProductCount)
            {
                // TODO: throw exception
                throw new ArgumentException("count must be > 0 and there must be enough space on the shelf");
            }

            // TODO: Check if user is trying to add different product to productNumber position

            // Don't check anything if shelf is empty and user is trying to add the first product.
            if (products[productNumber] != null)
            {
                Product existingProduct = products[productNumber].Item1;

                if (!existingProduct.Equals(product))
                {
                    // TODO: throw exception
                    throw new ArgumentException("You cannot add different products in one shelf");
                }
            }
        }

        public Product Buy(int productNumber)
        {
            throw new NotImplementedException();
        }

        public int GetProductAmount(int productNumber)
        {
            return products[productNumber].Item2;
        }

        public Money InsertCoin(Money amount)
        {
            if (amount.Euros <= 0 && amount.Cents <= 0)
            {
                throw new ArgumentOutOfRangeException("You must insert at least 10 cents");
            }

            //TODO: validate coins
            if (false == IsCoinValid(amount))
            {
                throw new ArgumentOutOfRangeException("Candy machine accepts only 10|20|50 cent and 1 euro coins");
            }
            Amount = MoneyHelper.AddMoney(Amount, amount);

            return Amount;
        }

        public Money ReturnMoney()
        {
            return Amount;
        }

        public static bool IsCoinValid(Money coin)
        {
            return acceptableCoins.Contains(coin);
        }
    }
}
