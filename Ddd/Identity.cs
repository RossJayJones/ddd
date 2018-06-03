using System;

namespace Ddd
{
    public class Identity
    {
        public Identity(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        public string Value { get; }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (ReferenceEquals(obj, this))
            {
                return true;
            }

            if (obj is string objAsString)
            {
                return Value.Equals(objAsString);
            }

            if (obj is Identity objAsIdentity)
            {
                return Value.Equals(objAsIdentity.Value);
            }

            return false;
        }

        public static bool operator ==(Identity lhs, Identity rhs)
        {
            return Equals(lhs, rhs);
        }

        public static bool operator !=(Identity lhs, Identity rhs)
        {
            return !(lhs == rhs);
        }
        
        public override string ToString()
        {
            return Value;
        }
    }
}
