using System;

namespace Sample.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message, Exception e = null) : base(message, e)
        {
            
        }
    }
}
