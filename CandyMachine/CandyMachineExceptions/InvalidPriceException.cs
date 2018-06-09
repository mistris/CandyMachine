﻿using System;

namespace CandyMachine.CandyMachineExceptions
{
    [Serializable]
    public class InvalidPriceException : Exception
    {
        public InvalidPriceException() { }
        public InvalidPriceException(string message) : base(message) { }
        public InvalidPriceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
