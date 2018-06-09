using CandyMachine.CandyMachineExceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyMachine
{
    public static class MoneyHelper
    {
        /// <summary>Adds given Money objects.</summary>
        /// <param name="amount">Current amount of money.</param>
        /// <param name="insertedMoney">Money that we want to add to current amount of money.</param>
        /// <returns>Returns new instance of Money.</returns>
        public static Money AddMoney(Money amount, Money insertedMoney)
        {
            int amountCents = ConvertMoneyToCents(amount);
            int insertedMoneyCents = ConvertMoneyToCents(insertedMoney);

            int resultCents = amountCents + insertedMoneyCents;

            return ConvertCentsToMoney(resultCents);
        }

        /// <summary>Subtracts one Money object from another.</summary>
        /// <param name="amount">Current amount of money.</param>
        /// <param name="price">Amount of money to be taken away from current amount of money.</param>
        /// <returns>Returns new instance of Money.</returns>
        public static Money SubtractMoney(Money amount, Money price)
        {
            int priceCents = ConvertMoneyToCents(price);
            int amountCents = ConvertMoneyToCents(amount);

            if (amountCents < priceCents)
            {
                throw new NegativeMoneyException("Price cannot be greater than current amount of money.");
            }

            int remainderCents = amountCents - priceCents;

            return ConvertCentsToMoney(remainderCents);
        }

        /// <summary>Converts instance of Money to cents.</summary>
        /// <param name="money">Instance of Money</param>
        /// <returns>Returns amount of cents.</returns>
        public static int ConvertMoneyToCents(Money money)
        {
            return money.Cents + money.Euros * 100;
        }

        /// <summary>Converts instance of Money to string.</summary>
        /// <param name="money">Instance of Money.</param>
        /// <returns>Returns string representing given instance of Money.</returns>
        public static string ConvertMoneyToString(Money money)
        {
            string moneyString = "";

            if (money.Euros > 0)
            {
                moneyString += money.Euros + " euros";

                if (money.Cents > 0)
                {
                    moneyString += " " + money.Cents + " cents";
                }
            }
            else
            {
                return money.Cents + " cents";
            }

            return moneyString;
        }

        /// <summary>Converts cents to instance of Money</summary>
        /// <param name="amount">Amount of cents</param>
        /// <returns>Returns new instance of Money.</returns>
        public static Money ConvertCentsToMoney(int amount)
        {
            return new Money { Euros = amount / 100, Cents = amount % 100 };
        }

        /// <summary>Converts List of Money objects to string.</summary>
        /// <param name="moneyList">List<Money></param>
        /// <returns>Returns string representing given List of Money objects.</returns>
        public static string ConvertMoneyListToString(IList<Money> moneyList)
        {
            // Sort money list ascending by cents and euros
            moneyList = moneyList.OrderBy(m => m.Euros).ThenBy(m => m.Cents).ToList();

            string moneyString = "";

            Money lastMoneyInList = moneyList.Last();

            // Build string of given Money objects
            foreach (Money price in moneyList)
            {
                if (price.Cents > 0)
                {
                    moneyString += price.Cents + " cent";
                }
                else if (price.Euros > 0)
                {
                    moneyString += price.Euros + " euro";
                }

                // Put pipe symbol after every coin element except the last one
                if (!price.Equals(lastMoneyInList))
                {
                    moneyString += " | ";
                }
            }

            return moneyString;
        }

        /// <summary>Checks if given price can be made of given acceptable coins.</summary>
        /// <param name="price">Price that we want to check.</param>
        /// <param name="acceptableCoins">Acceptable coins which we need to use to make given price.</param>
        /// <returns>Returns true if the price can be made of given acceptable coins.</returns>
        public static bool CanPriceBeMadeOfAcceptableCoins(Money price, IList<Money> acceptableCoins)
        {
            // Convert each acceptable coin to cents in ascending order
            IList<int> acceptableCoinsInAscendingOrder = acceptableCoins.Select(c => ConvertMoneyToCents(c)).OrderBy(m => m).ToList();

            // Convert each acceptable coin to cents in descending order
            IList<int> acceptableCoinsInDescendingOrder = acceptableCoins.Select(c => ConvertMoneyToCents(c)).OrderByDescending(m => m).ToList();

            // Convert given price in cents
            int priceInCents = ConvertMoneyToCents(price);
            int tempPriceInCents = priceInCents;

            // Subtract each acceptable coin from price while coin nominal value is less or equal than price 
            foreach (int coinValueInCents in acceptableCoinsInDescendingOrder)
            {
                while (priceInCents >= coinValueInCents)
                {
                    priceInCents -= coinValueInCents;
                }

                // If price in cents is 0, it means that we have made it from acceptable coins
                if (priceInCents == 0)
                {
                    return true;
                }
            }

            // If previous attempt wasn't successful, try to make given price of acceptable coins in reverse order
            foreach (int coinValueInCents in acceptableCoinsInAscendingOrder)
            {
                while (tempPriceInCents >= coinValueInCents)
                {
                    tempPriceInCents -= coinValueInCents;
                }

                if (tempPriceInCents == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
