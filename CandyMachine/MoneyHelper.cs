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

        /// <summary>Convert cents to Money</summary>
        /// <param name="amount">Amount of cents</param>
        /// <returns>Returns new instance of Money</returns>
        public static Money ConvertCentsToMoney(int amount)
        {
            return new Money { Euros = amount / 100, Cents = amount % 100 };
        }
    }
}
