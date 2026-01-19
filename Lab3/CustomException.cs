using System;

namespace Lab3
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException() : base("Invalid input detected.") { }
        public InvalidInputException(string message) : base(message) { }
        public InvalidInputException(string message, Exception inner) : base(message, inner) { }
    }
}