namespace Domain.Core.Base
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj is null || obj.GetType() != this.GetType())
                return false;
            var other = (ValueObject)obj;

            return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Aggregate(default(int), HashCode.Combine);
        }

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
            => !(left == right);
    }
}