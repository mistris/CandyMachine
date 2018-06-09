using System;

namespace CandyMachine.CandyMachineExceptions
{
    [Serializable]
    public class InvalidProductNumberException : Exception
    {
        public InvalidProductNumberException() { }
        public InvalidProductNumberException(string message) : base(message) { }
        public InvalidProductNumberException(string message, Exception innerException) : base(message, innerException) { }
    }
}
