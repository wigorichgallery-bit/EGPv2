namespace Platform.SharedKernel.UnitTests.Results;

public sealed class ErrorTypeTests
{
    [Fact]
    public void None_ShouldHaveExpectedValue()
    {
        // Assert
        ((int)ErrorType.None).Should().Be(0);
    }

    [Fact]
    public void Validation_ShouldHaveExpectedValue()
    {
        // Assert
        ((int)ErrorType.Validation).Should().Be(1);
    }

    [Fact]
    public void Unauthorized_ShouldHaveExpectedValue()
    {
        // Assert
        ((int)ErrorType.Unauthorized).Should().Be(2);
    }

    [Fact]
    public void Forbidden_ShouldHaveExpectedValue()
    {
        // Assert
        ((int)ErrorType.Forbidden).Should().Be(3);
    }

    [Fact]
    public void NotFound_ShouldHaveExpectedValue()
    {
        // Assert
        ((int)ErrorType.NotFound).Should().Be(4);
    }

    [Fact]
    public void Conflict_ShouldHaveExpectedValue()
    {
        // Assert
        ((int)ErrorType.Conflict).Should().Be(5);
    }

    [Fact]
    public void Internal_ShouldHaveExpectedValue()
    {
        // Assert
        ((int)ErrorType.Internal).Should().Be(6);
    }
}