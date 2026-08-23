using System;

namespace Domain.Exceptions
{
    public sealed class EmptyBasketException : Exception
    {
        public EmptyBasketException(string basketId) : base($"Basket With Id :{basketId} is empty or not found")
        {
        }
    }
}
