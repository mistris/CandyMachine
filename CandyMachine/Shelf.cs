using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
