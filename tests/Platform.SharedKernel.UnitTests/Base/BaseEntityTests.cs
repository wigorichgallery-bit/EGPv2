using FluentAssertions;
using Platform.SharedKernel.Base;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Base;

public sealed class BaseEntityTests
{
    [Fact]
    public void Constructor_WithValidId_ShouldCreateEntity()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var entity = new TestEntity(id);

        // Assert
        entity.Id.Should().Be(id);
    }

    [Fact]
    public void Constructor_WithEmptyGuid_ShouldThrowArgumentException()
    {
        // Act
        var action = () => new TestEntity(Guid.Empty);

        // Assert
        var exception = action.Should()
            .Throw<ArgumentException>()
            .WithParameterName("id")
            .Which;

        exception.Message.Should().Contain("Guid cannot be empty.");
    }

    [Fact]
    public void Equals_WithSameTypeAndSameId_ShouldReturnTrue()
    {
        var id = Guid.NewGuid();

        var left = new TestEntity(id);
        var right = new TestEntity(id);

        left.Equals(right).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithSameTypeAndDifferentId_ShouldReturnFalse()
    {
        var left = new TestEntity(Guid.NewGuid());
        var right = new TestEntity(Guid.NewGuid());

        left.Equals(right).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        var id = Guid.NewGuid();

        var left = new TestEntity(id);
        var right = new OtherEntity(id);

        left.Equals(right).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        var entity = new TestEntity(Guid.NewGuid());

        entity.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithEqualEntities_ShouldReturnSameHashCode()
    {
        var id = Guid.NewGuid();

        var left = new TestEntity(id);
        var right = new TestEntity(id);

        left.GetHashCode().Should().Be(right.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithDifferentEntities_ShouldReturnDifferentHashCode()
    {
        var left = new TestEntity(Guid.NewGuid());
        var right = new TestEntity(Guid.NewGuid());

        left.GetHashCode().Should().NotBe(right.GetHashCode());
    }

    [Fact]
    public void EqualityOperator_WithEqualEntities_ShouldReturnTrue()
    {
        var id = Guid.NewGuid();

        var left = new TestEntity(id);
        var right = new TestEntity(id);

        (left == right).Should().BeTrue();
    }

    [Fact]
    public void EqualityOperator_WithDifferentEntities_ShouldReturnFalse()
    {
        var left = new TestEntity(Guid.NewGuid());
        var right = new TestEntity(Guid.NewGuid());

        (left == right).Should().BeFalse();
    }

    [Fact]
    public void InequalityOperator_WithEqualEntities_ShouldReturnFalse()
    {
        var id = Guid.NewGuid();

        var left = new TestEntity(id);
        var right = new TestEntity(id);

        (left != right).Should().BeFalse();
    }

    [Fact]
    public void InequalityOperator_WithDifferentEntities_ShouldReturnTrue()
    {
        var left = new TestEntity(Guid.NewGuid());
        var right = new TestEntity(Guid.NewGuid());

        (left != right).Should().BeTrue();
    }

    private sealed class TestEntity : BaseEntity
    {
        public TestEntity(Guid id)
            : base(id)
        {
        }
    }

    private sealed class OtherEntity : BaseEntity
    {
        public OtherEntity(Guid id)
            : base(id)
        {
        }
    }
}