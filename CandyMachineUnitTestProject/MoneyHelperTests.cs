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
    public class MoneyHelperTests
    {
        [TestMethod()]
        public void AddMoneyTest()
        {
            // Start with no money
            Money currentMoney = new Money { Euros = 0, Cents = 0 };

            // Add 20 cents
            currentMoney = MoneyHelper.AddMoney(currentMoney, new Money { Euros = 0, Cents = 20 });
            Assert.AreEqual(0, currentMoney.Euros);
            Assert.AreEqual(20, currentMoney.Cents);

            // Add more money to check if euros are calculated correctly
            currentMoney = MoneyHelper.AddMoney(currentMoney, new Money { Euros = 0, Cents = 20 });
            currentMoney = MoneyHelper.AddMoney(currentMoney, new Money { Euros = 0, Cents = 20 });
            currentMoney = MoneyHelper.AddMoney(currentMoney, new Money { Euros = 0, Cents = 50 });
            Assert.AreEqual(1, currentMoney.Euros);
            Assert.AreEqual(10, currentMoney.Cents);

            // Add 1 euro coin
            currentMoney = MoneyHelper.AddMoney(currentMoney, new Money { Euros = 1, Cents = 0 });
            Assert.AreEqual(2, currentMoney.Euros);
            Assert.AreEqual(10, currentMoney.Cents);
        }

        [TestMethod()]
        public void SubtractMoneyTest()
        {
            // Start with 3 euros
            Money currentMoney = new Money { Euros = 3, Cents = 0 };

            // Subtract 20 cents
            currentMoney = MoneyHelper.SubtractMoney(currentMoney, new Money { Euros = 0, Cents = 20 });
            Assert.AreEqual(2, currentMoney.Euros);
            Assert.AreEqual(80, currentMoney.Cents);

            // Subtract more money to check if euros are calculated correctly
            currentMoney = MoneyHelper.SubtractMoney(currentMoney, new Money { Euros = 0, Cents = 20 });
            currentMoney = MoneyHelper.SubtractMoney(currentMoney, new Money { Euros = 0, Cents = 20 });
            currentMoney = MoneyHelper.SubtractMoney(currentMoney, new Money { Euros = 0, Cents = 50 });
            Assert.AreEqual(1, currentMoney.Euros);
            Assert.AreEqual(90, currentMoney.Cents);

            // Subtract 1 euro coin
            currentMoney = MoneyHelper.SubtractMoney(currentMoney, new Money { Euros = 1, Cents = 0 });
            Assert.AreEqual(0, currentMoney.Euros);
            Assert.AreEqual(90, currentMoney.Cents);

            // Subtract too much money
            Assert.ThrowsException<Exception>(() => MoneyHelper.SubtractMoney(currentMoney, new Money { Euros = 1, Cents = 0 }));
        }
    }
}