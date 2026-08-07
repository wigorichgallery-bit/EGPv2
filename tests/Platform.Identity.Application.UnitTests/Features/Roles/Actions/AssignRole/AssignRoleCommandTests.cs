using FluentAssertions;
using Platform.Identity.Application.Features.Roles.Actions;
using Xunit;

namespace Platform.Identity.Application.UnitTests.Features.Roles.Actions;

/// <summary>
/// Unit tests for <see cref="AssignRoleCommand"/>.
/// </summary>
public sealed class AssignRoleCommandTests
{
    /// <summary>
    /// Verifies constructor initializes every property.
    /// </summary>
    [Fact]
    public void Constructor_Should_Initialize_All_Properties()
    {
        // Arrange

        Guid userId =
            Guid.NewGuid();

        Guid roleId =
            Guid.NewGuid();

        // Act

        var command =
            new AssignRoleCommand(
                userId,
                roleId);

        // Assert

        command.UserId
            .Should()
            .Be(userId);

        command.RoleId
            .Should()
            .Be(roleId);
    }

    /// <summary>
    /// Verifies governance metadata is correct.
    /// </summary>
    [Fact]
    public void Governance_Metadata_Should_Be_Correct()
    {
        // Arrange

        var command =
            new AssignRoleCommand(
                Guid.NewGuid(),
                Guid.NewGuid());

        // Assert

        command.GovernancePolicy
            .Should()
            .Be("IDENTITY.ROLE.ASSIGN");

        command.Resource
            .Should()
            .Be("Role");

        command.Action
            .Should()
            .Be("Assign");
    }

    /// <summary>
    /// Verifies record equality for identical values.
    /// </summary>
    [Fact]
    public void Equality_Should_Return_True_For_Identical_Values()
    {
        // Arrange

        Guid userId =
            Guid.Parse(
                "11111111-1111-1111-1111-111111111111");

        Guid roleId =
            Guid.Parse(
                "22222222-2222-2222-2222-222222222222");

        var first =
            new AssignRoleCommand(
                userId,
                roleId);

        var second =
            new AssignRoleCommand(
                userId,
                roleId);

        // Assert

        first.Should().Be(second);
    }

    /// <summary>
    /// Verifies record inequality when values differ.
    /// </summary>
    [Fact]
    public void Equality_Should_Return_False_When_Values_Differ()
    {
        // Arrange

        var first =
            new AssignRoleCommand(
                Guid.NewGuid(),
                Guid.NewGuid());

        var second =
            new AssignRoleCommand(
                Guid.NewGuid(),
                Guid.NewGuid());

        // Assert

        first.Should().NotBe(second);
    }
}