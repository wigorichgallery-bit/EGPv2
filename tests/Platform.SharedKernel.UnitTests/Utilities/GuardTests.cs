namespace Platform.SharedKernel.UnitTests.Utilities;

/// <summary>
/// Contains unit tests for the <see cref="Guard"/> utility class.
/// </summary>
/// <remarks>
/// <para>
/// This test suite verifies that every guard clause correctly enforces
/// the defensive programming rules defined by the production implementation.
/// </para>
///
/// <para>
/// Each test follows the Arrange-Act-Assert (AAA) pattern and validates
/// exactly one observable behavior. Both successful execution paths and
/// exceptional execution paths are covered to ensure the guard methods
/// consistently protect domain invariants.
/// </para>
///
/// <para>
/// Covered guard methods:
/// </para>
/// <list type="bullet">
/// <item><description><see cref="Guard.AgainstNull{T}(T?, string)"/></description></item>
/// <item><description><see cref="Guard.AgainstNullOrWhiteSpace(string?, string)"/></description></item>
/// <item><description><see cref="Guard.AgainstEmpty(Guid, string)"/></description></item>
/// <item><description><see cref="Guard.AgainstFalse(bool, string)"/></description></item>
/// <item><description><see cref="Guard.AgainstNonUtc(DateTime, string)"/></description></item>
/// <item><description><see cref="Guard.AgainstUndefinedEnum{TEnum}(TEnum, string)"/></description></item>
/// <item><description><see cref="Guard.AgainstNegative(int, string)"/></description></item>
/// <item><description><see cref="Guard.AgainstNegativeOrZero(int, string)"/></description></item>
/// <item><description><see cref="Guard.AgainstEmptyCollection{T}(IEnumerable{T}?, string)"/></description></item>
/// <item><description><see cref="Guard.AgainstOutOfRange{T}(T, T, T, string)"/></description></item>
/// </list>
/// </remarks>
public sealed class GuardTests
{
    #region Guard.AgainstNull()

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstNull{T}(T?, string)"/>
    /// completes successfully when the supplied value is not
    /// <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// No exception is thrown.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstNull{T}(T?, string)"/>
    /// throws an <see cref="ArgumentNullException"/>
    /// when the supplied value is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentNullException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception contains the correct parameter name.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    #endregion

    #region Guard.AgainstNullOrWhiteSpace()

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstNullOrWhiteSpace(string?, string)"/>
    /// accepts a non-empty, non-whitespace string.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// No exception is thrown.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstNullOrWhiteSpace(string?, string)"/>
    /// throws an <see cref="ArgumentException"/> when the supplied value
    /// is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception contains the correct parameter name.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstNullOrWhiteSpace(string?, string)"/>
    /// throws an <see cref="ArgumentException"/> when the supplied value
    /// is an empty string.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentException"/> is thrown.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstNullOrWhiteSpace(string?, string)"/>
    /// throws an <see cref="ArgumentException"/> when the supplied value
    /// consists only of whitespace characters.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentException"/> is thrown.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    #region Guard.AgainstEmpty(Guid)

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstEmpty(Guid, string)"/>
    /// accepts a non-empty <see cref="Guid"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// No exception is thrown.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstEmpty(Guid, string)"/>
    /// throws an <see cref="ArgumentException"/> when the supplied
    /// <see cref="Guid"/> is <see cref="Guid.Empty"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception contains the correct parameter name and message.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    #endregion

    #region Guard.AgainstFalse(bool)

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstFalse(bool, string)"/>
    /// completes successfully when the supplied condition evaluates
    /// to <see langword="true"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// No exception is thrown.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstFalse(bool, string)"/>
    /// throws an <see cref="ArgumentException"/> when the supplied
    /// condition evaluates to <see langword="false"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception contains the expected error message.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    #region Guard.AgainstNonUtc(DateTime)

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstNonUtc(DateTime, string)"/>
    /// accepts a <see cref="DateTime"/> whose
    /// <see cref="DateTime.Kind"/> is <see cref="DateTimeKind.Utc"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// No exception is thrown.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstNonUtc(DateTime, string)"/>
    /// throws an <see cref="ArgumentException"/> when the supplied
    /// <see cref="DateTime"/> is not expressed in UTC.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception contains the correct parameter name and message.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    #region Guard.AgainstUndefinedEnum<TEnum>()

    /// <summary>
    /// Represents a sample enumeration used to verify
    /// <see cref="Guard.AgainstUndefinedEnum{TEnum}(TEnum, string)"/>.
    /// </summary>
    private enum TestEnum
    {
        First = 1,
        Second = 2
    }

    /// <summary>
    /// Verifies that
    /// <see cref="Guard.AgainstUndefinedEnum{TEnum}(TEnum, string)"/>
    /// accepts a defined enumeration value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// No exception is thrown.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that
    /// <see cref="Guard.AgainstUndefinedEnum{TEnum}(TEnum, string)"/>
    /// throws an <see cref="ArgumentException"/> when the supplied
    /// enumeration value is not defined.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception message identifies the undefined enumeration value.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    #region Guard.AgainstNegative(int)

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstNegative(int, string)"/>
    /// accepts a positive integer value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// No exception is thrown.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstNegative(int, string)"/>
    /// throws an <see cref="ArgumentOutOfRangeException"/> when the supplied
    /// value is less than zero.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentOutOfRangeException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception contains the correct parameter name and error message.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    #region Guard.AgainstNegativeOrZero(int)

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstNegativeOrZero(int, string)"/>
    /// accepts a positive integer value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// No exception is thrown.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstNegativeOrZero(int, string)"/>
    /// throws an <see cref="ArgumentOutOfRangeException"/> when the supplied
    /// value is equal to zero.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentOutOfRangeException"/> is thrown.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstNegativeOrZero(int, string)"/>
    /// throws an <see cref="ArgumentOutOfRangeException"/> when the supplied
    /// value is less than zero.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentOutOfRangeException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception contains the expected parameter name and message.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    #region Guard.AgainstEmptyCollection<T>()

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstEmptyCollection{T}(IEnumerable{T}?, string)"/>
    /// accepts a collection containing one or more elements.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// No exception is thrown.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstEmptyCollection{T}(IEnumerable{T}?, string)"/>
    /// throws an <see cref="ArgumentException"/> when the supplied collection
    /// reference is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception contains the expected parameter name and message.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that <see cref="Guard.AgainstEmptyCollection{T}(IEnumerable{T}?, string)"/>
    /// throws an <see cref="ArgumentException"/> when the supplied collection
    /// contains no elements.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception contains the expected parameter name and message.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    #region Guard.AgainstOutOfRange<T>()

    /// <summary>
    /// Verifies that
    /// <see cref="Guard.AgainstOutOfRange{T}(T, T, T, string)"/>
    /// accepts a value that falls within the specified inclusive range.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// No exception is thrown when the supplied value is between the
    /// minimum and maximum boundaries.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that
    /// <see cref="Guard.AgainstOutOfRange{T}(T, T, T, string)"/>
    /// throws an <see cref="ArgumentOutOfRangeException"/> when the supplied
    /// value is less than the minimum allowed value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentOutOfRangeException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception contains the correct parameter name and descriptive
    /// validation message.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Verifies that
    /// <see cref="Guard.AgainstOutOfRange{T}(T, T, T, string)"/>
    /// throws an <see cref="ArgumentOutOfRangeException"/> when the supplied
    /// value exceeds the maximum allowed value.
    /// </summary>
    /// <remarks>
    /// Expected Result:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// An <see cref="ArgumentOutOfRangeException"/> is thrown.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// The exception contains the correct parameter name and descriptive
    /// validation message.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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