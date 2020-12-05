using System;
namespace gamitude_backend.Exceptions
{
    public class ShopException : Exception
    {
        public ShopException(string message) : base(message)
        {
        }
    }
}
