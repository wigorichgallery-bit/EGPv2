namespace Platform.SharedKernel.UnitTests.Utilities;

public sealed class GuardTests
{
    [Fact]
    public void AgainstNull_WithNonNullValue_ShouldNotThrow()
    {
        // Arrange
        var value = "ChatGPT";

        // Act
        var action = () => Guard.AgainstNull(value, nameof(value));

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void AgainstNull_WithNullValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var action = () => Guard.AgainstNull(value, nameof(value));

        // Assert
        action.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName(nameof(value));
    }

    #region Implementasi AgainstNullOrWhiteSpace

        [Fact]
        public void AgainstNullOrWhiteSpace_WithValidValue_ShouldNotThrow()
        {
            // Arrange
            var value = "administrator";

            // Act
            var action = () => Guard.AgainstNullOrWhiteSpace(value, nameof(value));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void AgainstNullOrWhiteSpace_WithNullValue_ShouldThrowArgumentException()
        {
            // Arrange
            string? value = null;

            // Act
            var action = () => Guard.AgainstNullOrWhiteSpace(value, nameof(value));

            // Assert
            action.Should()
                .Throw<ArgumentException>()
                .WithParameterName(nameof(value));
        }

        [Fact]
        public void AgainstNullOrWhiteSpace_WithEmptyValue_ShouldThrowArgumentException()
        {
            // Arrange
            var value = string.Empty;

            // Act
            var action = () => Guard.AgainstNullOrWhiteSpace(value, nameof(value));

            // Assert
            action.Should()
                .Throw<ArgumentException>()
                .WithParameterName(nameof(value));
        }

        [Fact]
        public void AgainstNullOrWhiteSpace_WithWhitespaceValue_ShouldThrowArgumentException()
        {
            // Arrange
            var value = "   ";

            // Act
            var action = () => Guard.AgainstNullOrWhiteSpace(value, nameof(value));

            // Assert
            action.Should()
                .Throw<ArgumentException>()
                .WithParameterName(nameof(value));
        }

    #endregion

    #region Implementasi AgainstEmpty(Guid)

        [Fact]
        public void AgainstEmpty_WithValidGuid_ShouldNotThrow()
        {
            // Arrange
            var value = Guid.NewGuid();

            // Act
            var action = () => Guard.AgainstEmpty(value, nameof(value));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void AgainstEmpty_WithEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var value = Guid.Empty;

            // Act
            var action = () => Guard.AgainstEmpty(value, nameof(value));

            // Assert
            action.Should()
                .Throw<ArgumentException>()
                .WithParameterName(nameof(value))
                .WithMessage("Guid cannot be empty.*");
        }

        // Implementasi AgainstFalse(bool)
        [Fact]
        public void AgainstFalse_WithTrueCondition_ShouldNotThrow()
        {
            // Arrange
            const bool condition = true;

            // Act
            var action = () => Guard.AgainstFalse(condition, "Condition failed.");

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void AgainstFalse_WithFalseCondition_ShouldThrowArgumentException()
        {
            // Arrange
            const string message = "Condition failed.";

            // Act
            var action = () => Guard.AgainstFalse(false, message);

            // Assert
            action.Should()
                .Throw<ArgumentException>()
                .WithMessage(message);
        }

    #endregion

    #region Implementasi AgainstNonUtc(DateTime)

        [Fact]
        public void AgainstNonUtc_WithUtcDateTime_ShouldNotThrow()
        {
            // Arrange
            var value = DateTime.UtcNow;

            // Act
            var action = () => Guard.AgainstNonUtc(value, nameof(value));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void AgainstNonUtc_WithLocalDateTime_ShouldThrowArgumentException()
        {
            // Arrange
            var value = DateTime.Now;

            // Act
            var action = () => Guard.AgainstNonUtc(value, nameof(value));

            // Assert
            action.Should()
                .Throw<ArgumentException>()
                .WithParameterName(nameof(value))
                .WithMessage("Timestamp must be expressed in UTC.*");
        }

    #endregion

    #region Implementasi AgainstUndefinedEnum<TEnum>()

        private enum TestEnum
        {
            First = 1,
            Second = 2
        }

        [Fact]
        public void AgainstUndefinedEnum_WithDefinedValue_ShouldNotThrow()
        {
            // Arrange
            var value = TestEnum.First;

            // Act
            var action = () => Guard.AgainstUndefinedEnum(value, nameof(value));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void AgainstUndefinedEnum_WithUndefinedValue_ShouldThrowArgumentException()
        {
            // Arrange
            var value = (TestEnum)999;

            // Act
            var action = () => Guard.AgainstUndefinedEnum(value, nameof(value));

            // Assert
            var exception = action.Should()
                .Throw<ArgumentException>()
                .WithParameterName(nameof(value))
                .Which;

            exception.Message.Should().Contain("Undefined enum value");
            exception.Message.Should().Contain("999");
        }

    #endregion

    #region Implementasi AgainstNegative(int)

        [Fact]
        public void AgainstNegative_WithPositiveValue_ShouldNotThrow()
        {
            // Arrange
            const int value = 10;

            // Act
            var action = () => Guard.AgainstNegative(value, nameof(value));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void AgainstNegative_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            const int value = -1;

            // Act
            var action = () => Guard.AgainstNegative(value, nameof(value));

            // Assert
            var exception = action.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithParameterName(nameof(value))
                .Which;

            exception.Message.Should().Contain("Value cannot be negative.");
        }
    #endregion

    #region Implementasi AgainstNegativeOrZero(int)

        [Fact]
        public void AgainstNegativeOrZero_WithPositiveValue_ShouldNotThrow()
        {
            // Arrange
            const int value = 1;

            // Act
            var action = () => Guard.AgainstNegativeOrZero(value, nameof(value));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void AgainstNegativeOrZero_WithZeroValue_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            const int value = 0;

            // Act
            var action = () => Guard.AgainstNegativeOrZero(value, nameof(value));

            // Assert
            var exception = action.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithParameterName(nameof(value))
                .Which;

            exception.Message.Should().Contain("Value must be greater than zero.");
        }

        [Fact]
        public void AgainstNegativeOrZero_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            const int value = -10;

            // Act
            var action = () => Guard.AgainstNegativeOrZero(value, nameof(value));

            // Assert
            var exception = action.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithParameterName(nameof(value))
                .Which;

            exception.Message.Should().Contain("Value must be greater than zero.");
        }

    #endregion  

    #region Implementasi AgainstEmptyCollection<T>() 

        [Fact]
        public void AgainstEmptyCollection_WithItems_ShouldNotThrow()
        {
            // Arrange
            var collection = new[] { 1, 2, 3 };

            // Act
            var action = () => Guard.AgainstEmptyCollection(collection, nameof(collection));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void AgainstEmptyCollection_WithNullCollection_ShouldThrowArgumentException()
        {
            // Arrange
            IEnumerable<int>? collection = null;

            // Act
            var action = () => Guard.AgainstEmptyCollection(collection, nameof(collection));

            // Assert
            var exception = action.Should()
                .Throw<ArgumentException>()
                .WithParameterName(nameof(collection))
                .Which;

            exception.Message.Should().Contain("Collection cannot be null or empty.");
        }

        [Fact]
        public void AgainstEmptyCollection_WithEmptyCollection_ShouldThrowArgumentException()
        {
            // Arrange
            IEnumerable<int> collection = [];

            // Act
            var action = () => Guard.AgainstEmptyCollection(collection, nameof(collection));

            // Assert
            var exception = action.Should()
                .Throw<ArgumentException>()
                .WithParameterName(nameof(collection))
                .Which;

            exception.Message.Should().Contain("Collection cannot be null or empty.");
        }

    #endregion

    #region Implementasi AgainstOutOfRange<T>()

        [Fact]
        public void AgainstOutOfRange_WithValueInRange_ShouldNotThrow()
        {
            // Arrange
            const int value = 5;
            const int minimum = 1;
            const int maximum = 10;

            // Act
            var action = () => Guard.AgainstOutOfRange(
                value,
                minimum,
                maximum,
                nameof(value));

            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void AgainstOutOfRange_WithValueBelowMinimum_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            const int value = 0;
            const int minimum = 1;
            const int maximum = 10;

            // Act
            var action = () => Guard.AgainstOutOfRange(
                value,
                minimum,
                maximum,
                nameof(value));

            // Assert
            var exception = action.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithParameterName(nameof(value))
                .Which;

            exception.Message.Should().Contain("Value must be between 1 and 10.");
        }

        [Fact]
        public void AgainstOutOfRange_WithValueAboveMaximum_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            const int value = 11;
            const int minimum = 1;
            const int maximum = 10;

            // Act
            var action = () => Guard.AgainstOutOfRange(
                value,
                minimum,
                maximum,
                nameof(value));

            // Assert
            var exception = action.Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithParameterName(nameof(value))
                .Which;

            exception.Message.Should().Contain("Value must be between 1 and 10.");
        }
        
    #endregion

}