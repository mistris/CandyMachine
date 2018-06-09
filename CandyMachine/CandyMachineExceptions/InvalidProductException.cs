using System;

namespace CandyMachine.CandyMachineExceptions
{
    [Serializable]
    public class InvalidProductException : Exception
    {
        public InvalidProductException() { }
        public InvalidProductException(string message) : base(message) { }
        public InvalidProductException(string message, Exception innerException) : base(message, innerException) { }
    }
}
