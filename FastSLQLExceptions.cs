using System;

namespace FastSLQL
{
    internal class FastSLQLException : Exception
    {
        public FastSLQLException(string message)
            : base(message) { }
    }
}
