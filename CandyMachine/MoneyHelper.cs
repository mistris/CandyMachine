using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyMachine
{
    public static class MoneyHelper
    {
        public static Money AddMoney(Money amount, Money insertedMoney)
        {
            int amountCents = ConvertMoneyToCents(amount);
            int insertedMoneyCents = ConvertMoneyToCents(insertedMoney);

            int resultCents = amountCents + insertedMoneyCents;

            return ConvertCentsToMoney(resultCents);
        }

        /// <summary>Subtract one Money object from another</summary>
        /// <param name="amount">Current amount of money</param>
        /// <param name="price">Amount of money to be taken away from current amount of money</param>
        /// <returns>Returns new instance of Money</returns>
        public static Money SubtractMoney(Money amount, Money price)
        {
            int priceCents = ConvertMoneyToCents(price);
            int amountCents = ConvertMoneyToCents(amount);

            if (amountCents < priceCents)
            {
                throw new Exception("Price cannot be greater than current amount of money.");
            }

            int remainderCents = amountCents - priceCents;

            return ConvertCentsToMoney(remainderCents);
        }

        /// <summary>Converts Money to cents</summary>
        /// <param name="money">Instance of Money</param>
        /// <returns>Returns amount of cents</returns>
        public static int ConvertMoneyToCents(Money money)
        {
            return money.Cents + money.Euros * 100;
        }

        /// <summary>Converts Money object to string.</summary>
        /// <param name="money"></param>
        /// <returns></returns>
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

        /// <summary>Convert cents to Money</summary>
        /// <param name="amount">Amount of cents</param>
        /// <returns>Returns new instance of Money</returns>
        public static Money ConvertCentsToMoney(int amount)
        {
            return new Money { Euros = amount / 100, Cents = amount % 100 };
        }

        public static string ConvertMoneyListToString(IList<Money> moneyList)
        {
            // Sort money list ascending by cents and euros
            moneyList = moneyList.OrderBy(m => m.Euros).ThenBy(m => m.Cents).ToList();

            string moneyString = "";

            Money lastMoneyInList = moneyList.Last();

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

                if (!price.Equals(lastMoneyInList))
                {
                    moneyString += " | ";
                }
            }

            return moneyString;
        }

        /// <summary>Check if given price can be made of given acceptable coins.</summary>
        /// <param name="price">Price that we want to check.</param>
        /// <param name="acceptableCoins">Acceptable coins which we need to use to make given price.</param>
        /// <returns>Returns true if price can be made of given acceptable coins.</returns>
        public static bool CanPriceBeMadeOfAcceptableCoins(Money price, IList<Money> acceptableCoins)
        {
            // Convert each acceptable coin to cents
            IList<int> acceptableCoinsInAscendingOrder = acceptableCoins.Select(c => ConvertMoneyToCents(c)).OrderBy(m => m).ToList();
            IList<int> acceptableCoinsInDescendingOrder = acceptableCoins.Select(c => ConvertMoneyToCents(c)).OrderByDescending(m => m).ToList();

            // Convert price in cents
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
