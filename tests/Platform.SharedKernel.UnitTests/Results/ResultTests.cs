using FluentAssertions;
using Platform.SharedKernel.Results;
using Xunit;

namespace Platform.SharedKernel.UnitTests.Results;

public sealed class ResultTests
{
    #region Factory Method
        [Fact]
        public void Success_ShouldReturnSuccessfulResult()
        {
            // Act
            var result = Result.Success();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Error.Should().BeSameAs(Error.None);
        }

        [Fact]
        public void Failure_WithValidError_ShouldReturnFailureResult()
        {
            // Arrange
            var error = new Error(
                "VALIDATION",
                "Validation failed.",
                ErrorType.Validation);

            // Act
            var result = Result.Failure(error);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeSameAs(error);
        }

        [Fact]
        public void Failure_WithNullError_ShouldThrowArgumentNullException()
        {
            // Act
            var action = () => Result.Failure(null!);

            // Assert
            action.Should()
                .Throw<ArgumentNullException>()
                .WithParameterName("error");
        }
    #endregion

    #region Test Helper
        private sealed class TestResult : Result
        {
            public TestResult(bool isSuccess, Error error)
                : base(isSuccess, error)
            {
            }
        }
    #endregion

    #region Protected Constructor Invariants
        [Fact]
        public void Constructor_WithSuccessAndErrorNone_ShouldCreateResult()
        {
            // Act
            var result = new TestResult(true, Error.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Error.Should().BeSameAs(Error.None);
        }

        [Fact]
        public void Constructor_WithSuccessAndActualError_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var error = new Error("CODE", "Message");

            // Act
            var action = () => new TestResult(true, error);

            // Assert
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Success result must contain Error.None.*");
        }

        [Fact]
        public void Constructor_WithFailureAndErrorNone_ShouldThrowInvalidOperationException()
        {
            // Act
            var action = () => new TestResult(false, Error.None);

            // Assert
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Failure result must contain actual error.*");
        }

        [Fact]
        public void Constructor_WithNullError_ShouldThrowArgumentNullException()
        {
            // Act
            var action = () => new TestResult(true, null!);

            // Assert
            action.Should()
                .Throw<ArgumentNullException>()
                .WithParameterName("error");
        }

    #endregion
}