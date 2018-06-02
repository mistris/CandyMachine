using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyMachine
{
    public static class MoneyHelper
    {
        private static readonly IList<Money> acceptableCoins = new List<Money>() {
            new Money { Euros = 0, Cents = 10 },
            new Money { Euros = 0, Cents = 20 },
            new Money { Euros = 0, Cents = 50 },
            new Money { Euros = 1, Cents = 0 }
        };

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

        public static bool IsCoinValid(Money coin)
        {
            return acceptableCoins.Contains(coin);
        }
    }
}
