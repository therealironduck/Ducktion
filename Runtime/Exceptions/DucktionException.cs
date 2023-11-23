using System;

namespace TheRealIronDuck.Ducktion.Exceptions
{
    public class DucktionException : Exception
    {
        public DucktionException(string message, Exception inner = null) : base(message, inner)
        {
        }
    }
}