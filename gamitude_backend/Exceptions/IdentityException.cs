using System;
using System.Collections.Generic;

namespace gamitude_backend.Exceptions
{
    public class IdentityException : Exception
    {
        public readonly IEnumerable<Microsoft.AspNetCore.Identity.IdentityError> errors;
        public IdentityException(IEnumerable<Microsoft.AspNetCore.Identity.IdentityError> errors) : base()
        {
            this.errors = errors;
        }
    }
}
