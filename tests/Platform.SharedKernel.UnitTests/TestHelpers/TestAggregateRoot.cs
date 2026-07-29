namespace Platform.SharedKernel.UnitTests.TestHelpers;

internal sealed class TestAggregateRoot : AggregateRoot
{
    public TestAggregateRoot(Guid id)
        : base(id)
    {
    }

    public void Add(DomainEvent domainEvent)
    {
        AddDomainEvent(domainEvent);
    }

    public void Clear()
    {
        ClearDomainEvents();
    }
}