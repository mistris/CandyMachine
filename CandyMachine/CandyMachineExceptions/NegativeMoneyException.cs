using System;

namespace CandyMachine.CandyMachineExceptions
{
    [Serializable]
    public class NegativeMoneyException : Exception
    {
        public NegativeMoneyException() { }
        public NegativeMoneyException(string message) : base(message) { }
        public NegativeMoneyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
