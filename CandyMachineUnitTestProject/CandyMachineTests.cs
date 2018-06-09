using Microsoft.VisualStudio.TestTools.UnitTesting;
using CandyMachine.CandyMachineExceptions;
using System;

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

            // Check if candy machine contains correct values for manufacturer and shelf count
            Assert.AreEqual(Manufacturer, candyMachine.Manufacturer);
            Assert.AreEqual(ShelfCount, candyMachine.ShelfCount);
        }

        [TestMethod()]
        public void AddValidProductTest()
        {
            // Create new candy machine and test product
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);
            Product product = new Product { Name = "Snickers", Price = new Money { Euros = 1, Cents = 50 } };

            int productNumber = 1;

            // Add product to candy machine
            candyMachine.AddProduct(product, 5, productNumber);

            // Check if candy machine contains newly added product
            Assert.IsTrue(candyMachine.HasProduct(productNumber, product));
        }

        [TestMethod()]
        public void AddProductWithInvalidProductNumberTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);
            Product product = new Product { Name = "Snickers", Price = new Money { Euros = 1, Cents = 50 } };
            
            Assert.ThrowsException<InvalidProductNumberException>(() => candyMachine.AddProduct(product, 3, -1));
            Assert.ThrowsException<InvalidProductNumberException>(() => candyMachine.AddProduct(product, 3, 10));
        }

        [TestMethod()]
        public void AddProductWithInvalidProductCountTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);
            Product product = new Product { Name = "Snickers", Price = new Money { Euros = 1, Cents = 50 } };

            // Try to add -1 products in a shelf
            Assert.ThrowsException<InvalidProductCountException>(() => candyMachine.AddProduct(product, -1, 1));

            // Try to add 6 products in a shelf which can contain only 5 products
            Assert.ThrowsException<InvalidProductCountException>(() => candyMachine.AddProduct(product, 6, 1));

            // Try to add 5 products and then 1 more
            candyMachine.AddProduct(product, 5, 1);
            Assert.ThrowsException<InvalidProductCountException>(() => candyMachine.AddProduct(product, 1, 1));
        }

        [TestMethod()]
        public void AddProductWithInvalidPriceTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);
            Product product = new Product { Name = "Snickers", Price = new Money { Euros = 1, Cents = 55 } };
            
            Assert.ThrowsException<InvalidPriceException>(() => candyMachine.AddProduct(product, 1, 1));
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
            
            Assert.ThrowsException<InvalidProductException>(() => candyMachine.AddProduct(differentProduct, 1, 1));
        }

        [TestMethod()]
        public void AppendValidProductToOneShelfTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            Money price = new Money { Euros = 1, Cents = 50 };
            Product product = new Product { Name = "Snickers", Price = price };

            int productNumber = 1;

            // Add 1 product in shelf so there is enough space for other products
            candyMachine.AddProduct(product, 1, productNumber);

            // Try to add one more valid product to the same shelf
            Product anotherProduct = new Product { Name = "Snickers", Price = price };
            candyMachine.AddProduct(anotherProduct, 1, productNumber);

            // Check if count for this product increased
            Assert.AreEqual(2, candyMachine.GetProductAmount(productNumber));
        }

        [TestMethod()]
        public void TryToAddProductWithoutNameTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);
            Product product = new Product { Price = new Money { Euros = 1, Cents = 50 } };
            
            Assert.ThrowsException<InvalidProductException>(() => candyMachine.AddProduct(product, 1, 1));
        }

        [TestMethod()]
        public void BuyProductWithEnoughMoneyTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);
            Product product = new Product { Name = "Snickers", Price = new Money { Euros = 1, Cents = 20 } };

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
            Assert.IsTrue(product.Equals(purchasedProduct));

            // Check if candy machine now contains 4 snickers
            Assert.AreEqual(4, candyMachine.GetProductAmount(productNumber));

            // Check if remainder is 80 cents
            Money remainder = candyMachine.ReturnMoney();
            Assert.AreEqual(0, remainder.Euros);
            Assert.AreEqual(80, remainder.Cents);
        }

        [TestMethod()]
        public void BuyTwoDifferentProductsAndGetRemainderTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);
            Product productSnickers = new Product { Name = "Snickers", Price = new Money { Euros = 1, Cents = 20 } };
            Product productTwix = new Product { Name = "Twix", Price = new Money { Euros = 1 } };

            // Add products to candy machine
            candyMachine.AddProduct(productSnickers, 5, 0);
            candyMachine.AddProduct(productTwix, 5, 1);

            // Insert 3 euros into candy machine
            candyMachine.InsertCoin(new Money { Euros = 1 });
            candyMachine.InsertCoin(new Money { Euros = 1 });
            candyMachine.InsertCoin(new Money { Euros = 1 });

            // Buy 1 snickers and 1 twix
            candyMachine.Buy(0);
            candyMachine.Buy(1);

            // Check if remainder is 80 cents
            Money remainder = candyMachine.ReturnMoney();
            Assert.AreEqual(0, remainder.Euros);
            Assert.AreEqual(80, remainder.Cents);
        }

        [TestMethod()]
        public void TryToBuyProductFromUnexistingShelfTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);
            Product product = new Product { Name = "Snickers", Price = new Money { Euros = 1 } };

            // Add 5 snickers into candy machine
            candyMachine.AddProduct(product, 5, 7);

            // Insert 1 euro into candy machine
            candyMachine.InsertCoin(new Money { Euros = 1 });

            // Try to buy product from unexisting shelfs
            Assert.ThrowsException<InvalidProductNumberException>(() => candyMachine.Buy(20));
            Assert.ThrowsException<InvalidProductNumberException>(() => candyMachine.Buy(-1));
        }

        [TestMethod()]
        public void TryToBuyProductsEdgeCaseTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            Product productSnickers = new Product { Name = "Snickers", Price = new Money { Cents = 80 } };
            Product productTwix = new Product { Name = "Twix", Price = new Money { Euros = 1 } };
            Product productBounty = new Product { Name = "Bounty", Price = new Money { Euros = 1, Cents = 10 } };

            // Add products to candy machine
            candyMachine.AddProduct(productSnickers, 5, 0);
            candyMachine.AddProduct(productTwix, 1, 1);
            candyMachine.AddProduct(productBounty, 2, 2);

            // Insert 3 euros into candy machine
            candyMachine.InsertCoin(new Money { Euros = 1 });
            candyMachine.InsertCoin(new Money { Euros = 1 });
            candyMachine.InsertCoin(new Money { Euros = 1 });

            // Buy product from the 1st shelf
            Product purchasedSnickers = candyMachine.Buy(0);
            Assert.IsTrue(productSnickers.Equals(purchasedSnickers));

            // Buy product from the last shelf
            Product purchasedBounty = candyMachine.Buy(2);
            Assert.IsTrue(productBounty.Equals(purchasedBounty));

            // Buy the last product from shelf
            Product purchasedTwix = candyMachine.Buy(1);
            Assert.IsTrue(productTwix.Equals(purchasedTwix));

            // Try to buy another product from the shelf which is now empty
            Assert.ThrowsException<ProductOutOfStockException>(() => candyMachine.Buy(1));
        }

        [TestMethod()]
        public void TryToBuyProductFromEmptyCandyMachineTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);

            Assert.ThrowsException<ProductOutOfStockException>(() => candyMachine.Buy(1));
        }

        [TestMethod()]
        public void TryToBuyProductWithInsufficientAndWithSufficientAmountOfMoneyTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);
            Product product = new Product { Name = "Snickers", Price = new Money { Euros = 1, Cents = 20 } };

            // Add 1 snickers into candy machine
            int count = 1;
            int productNumber = 7;
            candyMachine.AddProduct(product, count, productNumber);

            // Insert 1 euro into candy machine
            candyMachine.InsertCoin(new Money { Euros = 1 });

            // Buy 1 snicker which costs 1 euro 20 cents with only 1 euro
            Assert.ThrowsException<NotEnoughMoneyException>(() => candyMachine.Buy(productNumber));

            // Insert missing amount of money (20 cents) and try to buy the same product again
            candyMachine.InsertCoin(new Money { Cents = 20 });

            Product purchasedProduct = candyMachine.Buy(productNumber);

            // Check if candy machine gave us the correct product
            Assert.IsTrue(product.Equals(purchasedProduct));

            // Check if there are no more snickers in candy machine
            Assert.AreEqual(0, candyMachine.GetProductAmount(productNumber));

            // Check if there is no remainder
            Money remainder = candyMachine.ReturnMoney();
            Assert.AreEqual(0, remainder.Euros);
            Assert.AreEqual(0, remainder.Cents);
        }

        [TestMethod()]
        public void GetProductAmountTest()
        {
            CandyMachine candyMachine = new CandyMachine(Manufacturer, ShelfCount, ShelfSize);
            Product product = new Product { Name = "Snickers", Price = new Money { Euros = 1, Cents = 50 } };

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
            Assert.ThrowsException<InvalidCoinException>(() => candyMachine.InsertCoin(new Money { Cents = 1 }));
            Assert.ThrowsException<InvalidCoinException>(() => candyMachine.InsertCoin(new Money { Cents = 2 }));
            Assert.ThrowsException<InvalidCoinException>(() => candyMachine.InsertCoin(new Money { Cents = 5 }));
            Assert.ThrowsException<InvalidCoinException>(() => candyMachine.InsertCoin(new Money { Euros = 2 }));
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