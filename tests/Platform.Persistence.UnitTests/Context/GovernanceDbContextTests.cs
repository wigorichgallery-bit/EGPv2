// ===========================================
// File Location:
// tests/Platform.Persistence.UnitTests/
// Context/GovernanceDbContextTests.cs
// ===========================================

using Platform.Identity.Domain.Aggregates;
using Platform.Persistence.Context;

namespace Platform.Persistence.UnitTests.Context;

public sealed class GovernanceDbContextTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly GovernanceDbContext _dbContext;

    public GovernanceDbContextTests()
    {
        _connection = new SqliteConnection(
            "Data Source=:memory:");

        _connection.Open();

        var options =
            new DbContextOptionsBuilder<GovernanceDbContext>()
                .UseSqlite(_connection)
                .Options;

        _dbContext =
            new GovernanceDbContext(options);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenOptionsAreNull()
    {
        var act = () =>
            new GovernanceDbContext(null!);

        act.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void UserAccounts_ShouldReturnDbSet()
    {
        _dbContext.UserAccounts
            .Should()
            .NotBeNull();

        _dbContext.UserAccounts
            .EntityType
            .ClrType
            .Should()
            .Be<UserAccount>();
    }

    [Fact]
    public void Roles_ShouldReturnDbSet()
    {
        _dbContext.Roles
            .Should()
            .NotBeNull();

        _dbContext.Roles
            .EntityType
            .ClrType
            .Should()
            .Be<Role>();
    }

    [Fact]
    public void Model_ShouldContainUserAccountEntity()
    {
        var entityType =
            _dbContext.Model.FindEntityType(
                typeof(UserAccount));

        entityType
            .Should()
            .NotBeNull();
    }

    [Fact]
    public void Model_ShouldContainRoleEntity()
    {
        var entityType =
            _dbContext.Model.FindEntityType(
                typeof(Role));

        entityType
            .Should()
            .NotBeNull();
    }

    [Fact]
    public void Model_ShouldApplyEntityConfigurations()
    {
        var userEntity =
            _dbContext.Model.FindEntityType(
                typeof(UserAccount));

        var roleEntity =
            _dbContext.Model.FindEntityType(
                typeof(Role));

        userEntity
            .Should()
            .NotBeNull();

        roleEntity
            .Should()
            .NotBeNull();

        userEntity!
            .FindProperty(
                nameof(UserAccount.Username))
            .Should()
            .NotBeNull();

        roleEntity!
            .FindProperty(
                nameof(Role.Name))
            .Should()
            .NotBeNull();
    }

    [Fact]
    public void Database_ShouldBeCreatedSuccessfully()
    {
        var act = () =>
            _dbContext.Database.EnsureCreated();

        act.Should()
            .NotThrow();

        _dbContext.Database
            .CanConnect()
            .Should()
            .BeTrue();
    }

    [Fact]
    public void SaveChanges_ShouldReturnZero_WhenThereAreNoPendingChanges()
    {
        var result =
            _dbContext.SaveChanges();

        result
            .Should()
            .Be(0);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldReturnZero_WhenThereAreNoPendingChanges()
    {
        var result =
            await _dbContext.SaveChangesAsync();

        result
            .Should()
            .Be(0);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldWork_WhenDatabaseSchemaExists()
    {
        await _dbContext.Database
            .EnsureCreatedAsync();

        var result =
            await _dbContext.SaveChangesAsync();

        result
            .Should()
            .Be(0);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _connection.Dispose();
    }
}