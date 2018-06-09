﻿using System;

namespace CandyMachine.CandyMachineExceptions
{
    [Serializable]
    public class InvalidProductCountException : Exception
    {
        public InvalidProductCountException() { }
        public InvalidProductCountException(string message) : base(message) { }
        public InvalidProductCountException(string message, Exception innerException) : base(message, innerException) { }
    }
}
