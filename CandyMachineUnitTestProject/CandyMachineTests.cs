using Microsoft.VisualStudio.TestTools.UnitTesting;
using CandyMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyMachine.Tests
{
    [TestClass()]
    public class CandyMachineTests
    {
        private const string Manufacturer = "Saeco";
        private const int ShelfCount = 10;
        private const int ShelfSize = 5;

        [TestMethod()]
        public void CandyMachineTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            Assert.AreEqual(candyMachine.Manufacturer, Manufacturer);
            Assert.AreEqual(candyMachine.ShelfSize, ShelfSize);
            Assert.AreEqual(candyMachine.ShelfCount, ShelfCount);
        }


        [TestMethod()]
        public void AddValidProductTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            Money price = new Money { Euros = 1, Cents = 50 };
            Product product = new Product { Name = "Snickers", Price = price };

            int productNumber = 1;

            candyMachine.AddProduct(product, 5, productNumber);

            Assert.IsTrue(candyMachine.ContainsProduct(productNumber, product));
        }

        [TestMethod()]
        public void AddProductWithInvalidProductNumberTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            Money price = new Money { Euros = 1, Cents = 50 };
            Product product = new Product { Name = "Snickers", Price = price };

            Assert.ThrowsException<IndexOutOfRangeException>(() => candyMachine.AddProduct(product, 3, -1));
            Assert.ThrowsException<IndexOutOfRangeException>(() => candyMachine.AddProduct(product, 3, 10));
        }

        [TestMethod()]
        public void AddProductWithInvalidProductCountTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            Money price = new Money { Euros = 1, Cents = 50 };
            Product product = new Product { Name = "Snickers", Price = price };

            // Try to add -1 products in a shelf
            Assert.ThrowsException<ArgumentException>(() => candyMachine.AddProduct(product, -1, 1));

            // Try to add 6 products in a shelf which can contain only 5 products
            Assert.ThrowsException<ArgumentException>(() => candyMachine.AddProduct(product, 6, 1));

            // Try to add 5 products and then 1 more
            candyMachine.AddProduct(product, 5, 1);
            Assert.ThrowsException<ArgumentException>(() => candyMachine.AddProduct(product, 1, 1));
        }

        [TestMethod()]
        public void AddDifferentProductsInOneShelfTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            Money price = new Money { Euros = 1, Cents = 50 };
            Product product = new Product { Name = "Snickers", Price = price };

            // Add 1 product in shelf so there is enough space for other products
            candyMachine.AddProduct(product, 1, 1);

            // Try to add different product to the same shelf
            Product differentProduct = new Product { Name = "Twix", Price = price };
            Assert.ThrowsException<ArgumentException>(() => candyMachine.AddProduct(differentProduct, 1, 1));
        }

        [TestMethod()]
        public void BuyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetProductAmountTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            Money price = new Money { Euros = 1, Cents = 50 };
            Product product = new Product { Name = "Snickers", Price = price };

            // Add 4 products to the 3rd position of candy machine
            int count = 4;
            int productNumber = 3;

            candyMachine.AddProduct(product, count, productNumber);
            Assert.IsTrue(candyMachine.GetProductAmount(productNumber) == count);
        }

        [TestMethod()]
        public void InsertCoinTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReturnMoneyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsCoinValidTest()
        {
            // Check if all acceptable coins are valid
            Assert.IsTrue(CandyMachine.IsCoinValid(new Money { Euros = 0, Cents = 10 }));
            Assert.IsTrue(CandyMachine.IsCoinValid(new Money { Euros = 0, Cents = 20 }));
            Assert.IsTrue(CandyMachine.IsCoinValid(new Money { Euros = 0, Cents = 50 }));
            Assert.IsTrue(CandyMachine.IsCoinValid(new Money { Euros = 1, Cents = 0 }));
        }

        [TestMethod()]
        public void IsCoinInvalidTest()
        {
            // Check if candy machine doesn't accept invalid coins
            Assert.IsFalse(CandyMachine.IsCoinValid(new Money { Euros = 0, Cents = 1 }));
            Assert.IsFalse(CandyMachine.IsCoinValid(new Money { Euros = 0, Cents = 2 }));
            Assert.IsFalse(CandyMachine.IsCoinValid(new Money { Euros = 0, Cents = 5 }));
            Assert.IsFalse(CandyMachine.IsCoinValid(new Money { Euros = 2, Cents = 0 }));
            Assert.IsFalse(CandyMachine.IsCoinValid(new Money { Euros = 5, Cents = 20 }));
        }
    }
}