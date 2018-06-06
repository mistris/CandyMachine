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

        private void FillShelvesWithProducts(CandyMachine candyMachine)
        {
            candyMachine.AddProduct(new Product { Name = "Bounty", Price = new Money { Cents = 80 } }, ShelfSize, 0);
            candyMachine.AddProduct(new Product { Name = "Daim", Price = new Money { Euros = 1, Cents = 10 } }, ShelfSize, 1);
            candyMachine.AddProduct(new Product { Name = "KitKat", Price = new Money { Euros = 1 } }, ShelfSize, 2);
            candyMachine.AddProduct(new Product { Name = "Lion", Price = new Money { Cents = 70 } }, ShelfSize, 3);
            candyMachine.AddProduct(new Product { Name = "Mars", Price = new Money { Cents = 90 } }, ShelfSize, 4);
            candyMachine.AddProduct(new Product { Name = "MilkyWay", Price = new Money { Cents = 80 } }, ShelfSize, 5);
            candyMachine.AddProduct(new Product { Name = "Skittles", Price = new Money { Euros = 1, Cents = 60 } }, ShelfSize, 6);
            candyMachine.AddProduct(new Product { Name = "Snickers", Price = new Money { Euros = 1, Cents = 20 } }, ShelfSize, 7);
            candyMachine.AddProduct(new Product { Name = "Tupla", Price = new Money { Cents = 80 } }, ShelfSize, 8);
            candyMachine.AddProduct(new Product { Name = "Twix", Price = new Money { Cents = 70 } }, ShelfSize, 9);
        }

        [TestMethod()]
        public void CandyMachineTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            Assert.AreEqual(Manufacturer, candyMachine.Manufacturer);
            Assert.AreEqual(ShelfCount, candyMachine.ShelfCount);
        }

        [TestMethod()]
        public void AddValidProductTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            Money price = new Money { Euros = 1, Cents = 50 };
            Product product = new Product { Name = "Snickers", Price = price };

            int productNumber = 1;

            candyMachine.AddProduct(product, 5, productNumber);

            Assert.IsTrue(candyMachine.HasProduct(productNumber, product));
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
        public void BuyProductWithEnoughMoneyTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            Money price = new Money { Euros = 1, Cents = 20 };
            Product product = new Product { Name = "Snickers", Price = price };

            // Add 5 snickers into candy machine
            int count = 5;
            int productNumber = 7;
            candyMachine.AddProduct(product, count, productNumber);

            // Insert 2 euros into candy machine
            candyMachine.InsertCoin(new Money { Euros = 1 });
            candyMachine.InsertCoin(new Money { Euros = 1 });

            // Buy 1 snickers which costs 1 euro 20 cents
            Product purchasedProduct = candyMachine.Buy(productNumber);

            // Check if candy machine gave us the correct product
            Assert.AreEqual("Snickers", purchasedProduct.Name);
            Assert.AreEqual(1, purchasedProduct.Price.Euros);
            Assert.AreEqual(20, purchasedProduct.Price.Cents);

            // Check if candy machine now contains 4 snickers
            Assert.AreEqual(4, candyMachine.GetProductAmount(productNumber));

            // Check if remainder is 80 cents
            Money remainder = candyMachine.ReturnMoney();
            Assert.AreEqual(0, remainder.Euros);
            Assert.AreEqual(80, remainder.Cents);
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
        public void InsertValidCoinsTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            // Insert 10 cent coin and check if candy machine contains 10 cents
            candyMachine.InsertCoin(new Money { Cents = 10 });
            Assert.AreEqual(10, candyMachine.Amount.Cents);

            // Insert 3x20 and 1x50 cent coins and check if candy machine now contains 1 euro 20 cents
            candyMachine.InsertCoin(new Money { Cents = 20 });
            candyMachine.InsertCoin(new Money { Cents = 20 });
            candyMachine.InsertCoin(new Money { Cents = 20 });
            candyMachine.InsertCoin(new Money { Cents = 50 });

            Assert.AreEqual(1, candyMachine.Amount.Euros);
            Assert.AreEqual(20, candyMachine.Amount.Cents);
        }

        [TestMethod()]
        public void InsertInvalidCoinsTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            // Insert invalid coins
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => candyMachine.InsertCoin(new Money { Cents = 1 }));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => candyMachine.InsertCoin(new Money { Cents = 2 }));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => candyMachine.InsertCoin(new Money { Cents = 5 }));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => candyMachine.InsertCoin(new Money { Euros = 2 }));
        }

        [TestMethod()]
        public void ReturnMoneyWithoutBuyingProductsTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            // Insert 20 cents and check if candy machine can return them
            candyMachine.InsertCoin(new Money { Cents = 20 });
            Money remainder = candyMachine.ReturnMoney();

            Assert.AreEqual(0, remainder.Euros);
            Assert.AreEqual(20, remainder.Cents);

            // Insert 2x1 euro coins, 3x50 cent coins, 4x20 cent coins and 1x10 cent coin (Total: 4 euros 40 cents)
            candyMachine.InsertCoin(new Money { Euros = 1 });
            candyMachine.InsertCoin(new Money { Euros = 1 });

            candyMachine.InsertCoin(new Money { Cents = 50 });
            candyMachine.InsertCoin(new Money { Cents = 50 });
            candyMachine.InsertCoin(new Money { Cents = 50 });

            candyMachine.InsertCoin(new Money { Cents = 20 });
            candyMachine.InsertCoin(new Money { Cents = 20 });
            candyMachine.InsertCoin(new Money { Cents = 20 });
            candyMachine.InsertCoin(new Money { Cents = 20 });

            candyMachine.InsertCoin(new Money { Cents = 10 });

            // Check if candy machine returns 4 euros 40 cents
            remainder = candyMachine.ReturnMoney();
            Assert.AreEqual(4, remainder.Euros);
            Assert.AreEqual(40, remainder.Cents);
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