namespace Platform.SharedKernel.UnitTests.TestHelpers;

public sealed class TestValueObject : ValueObject
{
    public TestValueObject(string first, int second)
    {
        First = first;
        Second = second;
    }

    public string First { get; }

    public int Second { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return First;
        yield return Second;
    }
}

public sealed class OtherValueObject : ValueObject
{
    public OtherValueObject(string value)
    {
        Value = value;
    }

    public string Value { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}