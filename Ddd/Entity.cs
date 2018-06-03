using System;

namespace Ddd
{
    public class Entity<TIdentity> where TIdentity : Identity
    {
        public TIdentity Id { get; protected set; }

        public override int GetHashCode()
        {
            if (Id == null)
            {
                throw new ArgumentNullException(nameof(Id));
            }

            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity<TIdentity> other))
            {
                return false;
            }

            return other.Id.Equals(Id);
        }

        public static bool operator ==(Entity<TIdentity> lhs, Entity<TIdentity> rhs)
        {
            return lhs?.Id == rhs?.Id;
        }

        public static bool operator !=(Entity<TIdentity> lhs, Entity<TIdentity> rhs)
        {
            return lhs?.Id != rhs?.Id;
        }
    }
}
