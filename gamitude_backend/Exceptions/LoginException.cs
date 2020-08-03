using System;
namespace gamitude_backend.Exceptions
{
    public class LoginException : Exception
    {
        public LoginException(String message) : base(message)
        {
        }
    }
}
