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
            amount.Euros += insertedMoney.Euros;
            amount.Cents += insertedMoney.Cents;

            if (amount.Cents >= 100)
            {
                amount.Euros += amount.Cents / 100;
                amount.Cents %= 100;
            }

            return amount;
        }

        public static Money SubtractMoney(Money amount, Money price)
        {
            return new Money();
        }
    }
}
