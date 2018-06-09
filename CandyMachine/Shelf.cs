using CandyMachine.CandyMachineExceptions;
using System;

namespace CandyMachine
{
    class Shelf
    {
        /// <summary>Number of products you can store in one shelf.</summary>
        public int ShelfSize { get; private set; }

        /// <summary></summary>
        public int ProductCount { get; private set; }

        /// <summary>Product that is stored in this specific shelf.</summary>
        public Product? Product { get; private set; }

        public Shelf(int shelfSize)
        {
            ShelfSize = shelfSize;
            ProductCount = 0;
        }

        /// <summary>Checks if shelf contains given product.</summary>
        /// <param name="product">Product that we want to find.</param>
        /// <returns>Returns true if shelf contains given product.</returns>
        public bool ContainsProduct(Product product)
        {
            if (!IsEmpty())
            {
                return Product.Equals(product);
            }

            return false;
        }

        /// <summary>Adds a product to the shelf.</summary>
        /// <param name="product">Product that is sold.</param>
        /// <param name="count">How many products do we want to add.</param>
        public void AddProduct(Product product, int count)
        {
            ValidateProduct(product, count);
            Product = product;
            ProductCount += count;
        }
        
        public void DecreaseProductCount()
        {
            ProductCount--;
        }

        /// <summary>Checks if this specific shelf is empty.</summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Product == null || ProductCount == 0;
        }

        /// <summary>
        /// Checks if there is enough space in specified candy machine position (shelf number).
        /// Checks if user is trying to add different products in one shelf.
        /// Checks if product has name.
        /// </summary>
        /// <param name="product">Product that we want to add.</param>
        /// <param name="count">Units of product.</param>
        private void ValidateProduct(Product product, int count)
        {
            // Check if there is enough space for product in specified productNumber position
            int spaceLeftInShelf = ShelfSize - ProductCount;

            if (spaceLeftInShelf == 0)
            {
                throw new InvalidProductCountException("Shelf is full. You cannot add more products to it.");
            }
            else
            {
                if (count < 1 || count > spaceLeftInShelf)
                {
                    throw new InvalidProductCountException($"Product count must be > 0 and <= {spaceLeftInShelf}");
                }
            }

            // Check if user is trying to add different product to productNumber position
            if (Product != null)
            {
                if (!Product.Equals(product))
                {
                    throw new InvalidProductException("You cannot add different products in one shelf.");
                }
            }
            
            // Check if product has name
            if (String.IsNullOrEmpty(product.Name))
            {
                throw new InvalidProductException("Product must contain a name.");
            }
        }
    }
}
