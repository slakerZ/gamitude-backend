using System;
namespace gamitude_backend.Exceptions
{
    public class LoginException : Exception
    {
        public LoginException(string message) : base(message)
        {
        }
    }
}
