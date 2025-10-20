using Domain.Core.Base;
using System.Numerics;

public abstract class QuantityBase<TNumber, TSelf> : ValueObject
    where TNumber : struct, INumber<TNumber>
    where TSelf : QuantityBase<TNumber, TSelf>
{
    public TNumber Value { get; }

    protected QuantityBase(TNumber value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    protected abstract TSelf CreateCore(TNumber value);

    public TSelf Add(TSelf delta)
    {
        return CreateCore(Value + delta.Value);
    }

    public TSelf Subtract(TSelf delta)
    {
        return CreateCore(Value - delta.Value);
    }

    public static TSelf operator +(QuantityBase<TNumber, TSelf> left, QuantityBase<TNumber, TSelf> right)
    {
        return left.CreateCore(left.Value + right.Value);
    }

    public static TSelf operator -(QuantityBase<TNumber, TSelf> left, QuantityBase<TNumber, TSelf> right)
    {
        return left.CreateCore(left.Value - right.Value);
    }

    public static implicit operator TNumber(QuantityBase<TNumber, TSelf> quantityBase) => quantityBase.Value;
}