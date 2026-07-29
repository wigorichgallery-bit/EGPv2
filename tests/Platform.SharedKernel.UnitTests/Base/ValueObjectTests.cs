using FluentAssertions;
using Platform.SharedKernel.Base;
using Platform.SharedKernel.UnitTests.TestHelpers;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Base;

public sealed class ValueObjectTests
{
    #region Equality
        [Fact]
        public void Equals_WithSameValues_ShouldReturnTrue()
        {
            // Arrange
            var left = new TestValueObject("ABC", 10);
            var right = new TestValueObject("ABC", 10);

            // Assert
            left.Equals(right).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithDifferentValues_ShouldReturnFalse()
        {
            var left = new TestValueObject("ABC", 10);
            var right = new TestValueObject("XYZ", 10);

            left.Equals(right).Should().BeFalse();
        }

        [Fact]
        public void Equals_WithDifferentType_ShouldReturnFalse()
        {
            var left = new TestValueObject("ABC", 10);
            var right = new OtherValueObject("ABC");

            left.Equals(right).Should().BeFalse();
        }

        [Fact]
        public void Equals_WithNull_ShouldReturnFalse()
        {
            var left = new TestValueObject("ABC", 10);

            left.Equals(null).Should().BeFalse();
        }
    #endregion

    #region HashCode
        [Fact]
        public void GetHashCode_WithEqualObjects_ShouldReturnSameHashCode()
        {
            var left = new TestValueObject("ABC", 10);
            var right = new TestValueObject("ABC", 10);

            left.GetHashCode().Should().Be(right.GetHashCode());
        }

        [Fact]
        public void GetHashCode_WithDifferentObjects_ShouldReturnDifferentHashCode()
        {
            var left = new TestValueObject("ABC", 10);
            var right = new TestValueObject("XYZ", 20);

            left.GetHashCode().Should().NotBe(right.GetHashCode());
        }
    #endregion

    #region Operators
        [Fact]
        public void EqualityOperator_WithEqualObjects_ShouldReturnTrue()
        {
            var left = new TestValueObject("ABC", 10);
            var right = new TestValueObject("ABC", 10);

            (left == right).Should().BeTrue();
        }

        [Fact]
        public void EqualityOperator_WithDifferentObjects_ShouldReturnFalse()
        {
            var left = new TestValueObject("ABC", 10);
            var right = new TestValueObject("XYZ", 10);

            (left == right).Should().BeFalse();
        }

        [Fact]
        public void InequalityOperator_WithEqualObjects_ShouldReturnFalse()
        {
            var left = new TestValueObject("ABC", 10);
            var right = new TestValueObject("ABC", 10);

            (left != right).Should().BeFalse();
        }

        [Fact]
        public void InequalityOperator_WithDifferentObjects_ShouldReturnTrue()
        {
            var left = new TestValueObject("ABC", 10);
            var right = new TestValueObject("XYZ", 10);

            (left != right).Should().BeTrue();
        }
    #endregion
}