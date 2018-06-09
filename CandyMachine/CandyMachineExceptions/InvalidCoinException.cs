using System;

namespace CandyMachine.CandyMachineExceptions
{
    [Serializable]
    public class InvalidCoinException : Exception
    {
        public InvalidCoinException() { }
        public InvalidCoinException(string message) : base(message) { }
        public InvalidCoinException(string message, Exception innerException) : base(message, innerException) { }
    }
}
