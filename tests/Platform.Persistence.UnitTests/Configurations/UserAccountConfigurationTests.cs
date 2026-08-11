// ===========================================
// File Location :
// tests/Platform.Persistence.UnitTests/
// Configurations/UserAccountConfigurationTests.cs
// ===========================================

using Microsoft.EntityFrameworkCore.Metadata;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.Enums;
using Platform.Identity.Domain.ValueObjects;
using Platform.Persistence.Configurations;
using Platform.Persistence.Context;

namespace Platform.Persistence.UnitTests.Configurations;

/// <summary>
/// Contains unit tests for the
/// <see cref="UserAccountConfiguration"/> class.
///
/// Responsibility:
/// - Verify UserAccount table mapping.
/// - Verify primary key configuration.
/// - Verify scalar property constraints.
/// - Verify unique username index.
/// - Verify MFA method conversion.
/// - Verify user status conversion.
/// - Verify Email owned value object mapping.
/// - Verify PhoneNumber owned value object mapping.
/// - Verify role assignment owned collection mapping.
/// - Verify role assignment foreign key.
/// - Verify role assignment composite key.
/// - Verify role assignment backing field access.
/// - Verify ignored domain event mapping.
///
/// Testing Strategy:
/// - Exercise the public Configure method.
/// - Inspect EF Core model metadata.
/// - Avoid database connections.
/// - Avoid reflection-based invocation of private methods.
/// - Verify private configuration behavior through
///   the resulting EF Core model.
/// </summary>
public sealed class UserAccountConfigurationTests
{
    /// <summary>
    /// Creates an EF Core model containing the
    /// <see cref="UserAccountConfiguration"/> mapping.
    ///
    /// The model is created in memory so persistence
    /// metadata can be verified without a database.
    /// </summary>
    /// <returns>
    /// The configured mutable EF Core model.
    /// </returns>
    private static IModel CreateModel()
    {
        var options =
            new DbContextOptionsBuilder<GovernanceDbContext>()
                .UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=EGPv2_UnitTest;Trusted_Connection=True;TrustServerCertificate=True")
                .Options;

        using var context =
            new GovernanceDbContext(options);

        return context.Model;
    }

    [Fact]
    public void Diagnostic_Should_ReadFinalGovernanceDbContextMetadata()
    {
        // Arrange
        var options =
            new DbContextOptionsBuilder<GovernanceDbContext>()
                .UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=EGPv2_UnitTest;Trusted_Connection=True;TrustServerCertificate=True")
                .Options;

        using var context =
            new GovernanceDbContext(options);

        // Act
        var model = context.Model;

        var entityType =
            model.FindEntityType(typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        var mfaProperty =
            entityType!
                .FindProperty(nameof(UserAccount.MFAMethod));

        var statusProperty =
            entityType
                .FindProperty(nameof(UserAccount.Status));

        var roleNavigation =
            entityType
                .FindNavigation(
                    nameof(UserAccount.RoleAssignments));

        // Assert
        mfaProperty.Should()
            .NotBeNull();

        statusProperty.Should()
            .NotBeNull();

        roleNavigation.Should()
            .NotBeNull();

        Console.WriteLine(
            $"MFA converter: " +
            $"{mfaProperty!.GetValueConverter()?.GetType().FullName}");

        Console.WriteLine(
            $"MFA mapping converter: " +
            $"{mfaProperty.GetTypeMapping().Converter?.GetType().FullName}");

        Console.WriteLine(
            $"Status converter: " +
            $"{statusProperty!.GetValueConverter()?.GetType().FullName}");

        Console.WriteLine(
            $"Status mapping converter: " +
            $"{statusProperty.GetTypeMapping().Converter?.GetType().FullName}");

        Console.WriteLine(
            $"Role field: " +
            $"{roleNavigation!.FieldInfo?.Name}");

        Console.WriteLine(
            $"Role field name: " +
            $"{roleNavigation.GetFieldName()}");

        Console.WriteLine(
            $"Role access mode: " +
            $"{roleNavigation.GetPropertyAccessMode()}");
    }

    /// <summary>
    /// Verifies that Configure throws
    /// <see cref="ArgumentNullException"/> when
    /// the supplied entity builder is null.
    /// </summary>
    [Fact]
    public void Configure_Should_ThrowArgumentNullException_When_BuilderIsNull()
    {
        // Arrange
        var sut = new UserAccountConfiguration();

        // Act
        Action act = () => sut.Configure(null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that UserAccount is mapped
    /// to the IdentityUsers table.
    /// </summary>
    [Fact]
    public void Configure_Should_MapUserAccountToIdentityUsersTable()
    {
        // Arrange
        var model = CreateModel();

        // Act
        var entityType = model.FindEntityType(
            typeof(UserAccount));

        // Assert
        entityType.Should()
            .NotBeNull();

        entityType!
            .GetTableName()
            .Should()
            .Be("IdentityUsers");
    }

    /// <summary>
    /// Verifies that UserAccount.Id is configured
    /// as the primary key and is never generated
    /// by the database.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureIdAsNonGeneratedPrimaryKey()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var primaryKey = entityType!
            .FindPrimaryKey();

        var idProperty = entityType.FindProperty(
            nameof(UserAccount.Id));

        // Assert
        primaryKey.Should()
            .NotBeNull();

        primaryKey!
            .Properties
            .Should()
            .ContainSingle();

        primaryKey.Properties[0]
            .Name
            .Should()
            .Be(nameof(UserAccount.Id));

        idProperty.Should()
            .NotBeNull();

        idProperty!
            .ValueGenerated
            .Should()
            .Be(ValueGenerated.Never);
    }

    /// <summary>
    /// Verifies that Username is required
    /// and has a maximum length of 100 characters.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureUsernameAsRequiredWithMaximumLength()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var property = entityType!
            .FindProperty(nameof(UserAccount.Username));

        // Assert
        property.Should()
            .NotBeNull();

        property!
            .IsNullable
            .Should()
            .BeFalse();

        property
            .GetMaxLength()
            .Should()
            .Be(100);
    }

    /// <summary>
    /// Verifies that Username has a unique
    /// database index.
    /// </summary>
    [Fact]
    public void Configure_Should_CreateUniqueIndexForUsername()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var indexes = entityType!
            .GetIndexes()
            .Where(index =>
                index.Properties.Count == 1
                && index.Properties[0].Name
                    == nameof(UserAccount.Username))
            .ToList();

        // Assert
        indexes.Should()
            .ContainSingle();

        indexes[0]
            .IsUnique
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Verifies that password and security
    /// properties use their configured lengths
    /// and required constraints.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureCredentialAndSecurityProperties()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var passwordHash = entityType!
            .FindProperty(nameof(UserAccount.PasswordHash));

        var securityStamp = entityType
            .FindProperty(nameof(UserAccount.SecurityStamp));

        var passwordVersion = entityType
            .FindProperty(nameof(UserAccount.PasswordVersion));

        var lastPasswordChangedAt = entityType
            .FindProperty(
                nameof(UserAccount.LastPasswordChangedAt));

        // Assert
        passwordHash.Should()
            .NotBeNull();

        passwordHash!
            .GetMaxLength()
            .Should()
            .Be(1000);

        passwordHash
            .IsNullable
            .Should()
            .BeFalse();

        securityStamp.Should()
            .NotBeNull();

        securityStamp!
            .GetMaxLength()
            .Should()
            .Be(100);

        securityStamp
            .IsNullable
            .Should()
            .BeFalse();

        passwordVersion.Should()
            .NotBeNull();

        passwordVersion!
            .IsNullable
            .Should()
            .BeFalse();

        lastPasswordChangedAt.Should()
            .NotBeNull();

        lastPasswordChangedAt!
            .IsNullable
            .Should()
            .BeFalse();
    }

    /// <summary>
    /// Verifies that required authentication state
    /// properties are configured as non-nullable.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureRequiredAuthenticationProperties()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var emailVerified = entityType!
            .FindProperty(nameof(UserAccount.EmailVerified));

        var phoneVerified = entityType
            .FindProperty(nameof(UserAccount.PhoneVerified));

        var mfaEnabled = entityType
            .FindProperty(nameof(UserAccount.MFAEnabled));

        var failedLoginCount = entityType
            .FindProperty(nameof(UserAccount.FailedLoginCount));

        // Assert
        emailVerified.Should()
            .NotBeNull();

        emailVerified!
            .IsNullable
            .Should()
            .BeFalse();

        phoneVerified.Should()
            .NotBeNull();

        phoneVerified!
            .IsNullable
            .Should()
            .BeFalse();

        mfaEnabled.Should()
            .NotBeNull();

        mfaEnabled!
            .IsNullable
            .Should()
            .BeFalse();

        failedLoginCount.Should()
            .NotBeNull();

        failedLoginCount!
            .IsNullable
            .Should()
            .BeFalse();
    }

    /// <summary>
    /// Verifies that MFAMethod uses a string
    /// value converter and the configured maximum
    /// length of 50 characters.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureMfaMethodConversion()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var property = entityType!
            .FindProperty(nameof(UserAccount.MFAMethod));

        // Assert
        property.Should()
            .NotBeNull();

        var converter = property!
            .GetTypeMapping()
            .Converter;

        converter.Should()
            .NotBeNull();

        converter!
            .ModelClrType
            .Should()
            .Be(typeof(MFAMethod));

        converter
            .ProviderClrType
            .Should()
            .Be(typeof(string));

        property
            .GetMaxLength()
            .Should()
            .Be(50);

        property
            .IsNullable
            .Should()
            .BeFalse();
    }

    /// <summary>
    /// Verifies that the MFAMethod converter
    /// converts the TOTP enum value into
    /// its string persistence representation.
    /// </summary>
    [Fact]
    public void Configure_Should_ConvertMfaMethodToString()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        var property = entityType!
            .FindProperty(nameof(UserAccount.MFAMethod));

        property.Should()
            .NotBeNull();

        var converter = property!
            .GetTypeMapping()
            .Converter;

        converter.Should()
            .NotBeNull();

        // Act
        var converted =
            converter!
                .ConvertToProviderExpression
                .Compile()
                .DynamicInvoke(
                    MFAMethod.TOTP);

        // Assert
        converted.Should()
            .Be("TOTP");
    }

    /// <summary>
    /// Verifies that the MFAMethod converter
    /// converts a persisted string into the
    /// corresponding enum value.
    /// </summary>
    [Fact]
    public void Configure_Should_ConvertStringToMfaMethod()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        var property = entityType!
            .FindProperty(nameof(UserAccount.MFAMethod));

        property.Should()
            .NotBeNull();

        var converter = property!
            .GetTypeMapping()
            .Converter;

        converter.Should()
            .NotBeNull();

        // Act
        var converted =
            converter!
                .ConvertFromProviderExpression
                .Compile()
                .DynamicInvoke("TOTP");

        // Assert
        converted.Should()
            .Be(MFAMethod.TOTP);
    }

    /// <summary>
    /// Verifies that TOTPSecretEncrypted has
    /// a maximum length of 2000 characters and
    /// remains nullable.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureTotpSecretAsOptionalWithMaximumLength()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var property = entityType!
            .FindProperty(
                nameof(UserAccount.TOTPSecretEncrypted));

        // Assert
        property.Should()
            .NotBeNull();

        property!
            .GetMaxLength()
            .Should()
            .Be(2000);

        property
            .IsNullable
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Verifies that audit and last-login properties
    /// use their configured lengths and nullability.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureAuditAndLastLoginProperties()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var createdAt = entityType!
            .FindProperty(nameof(UserAccount.CreatedAt));

        var updatedAt = entityType
            .FindProperty(nameof(UserAccount.UpdatedAt));

        var lastLoginIp = entityType
            .FindProperty(nameof(UserAccount.LastLoginIp));

        var lastLoginCountry = entityType
            .FindProperty(nameof(UserAccount.LastLoginCountry));

        var lastDeviceFingerprint = entityType
            .FindProperty(
                nameof(UserAccount.LastDeviceFingerprint));

        var lastLatitude = entityType
            .FindProperty(nameof(UserAccount.LastLatitude));

        var lastLongitude = entityType
            .FindProperty(nameof(UserAccount.LastLongitude));

        // Assert
        createdAt.Should()
            .NotBeNull();

        createdAt!
            .IsNullable
            .Should()
            .BeFalse();

        updatedAt.Should()
            .NotBeNull();

        updatedAt!
            .IsNullable
            .Should()
            .BeFalse();

        lastLoginIp.Should()
            .NotBeNull();

        lastLoginIp!
            .GetMaxLength()
            .Should()
            .Be(100);

        lastLoginIp
            .IsNullable
            .Should()
            .BeTrue();

        lastLoginCountry.Should()
            .NotBeNull();

        lastLoginCountry!
            .GetMaxLength()
            .Should()
            .Be(100);

        lastLoginCountry
            .IsNullable
            .Should()
            .BeTrue();

        lastDeviceFingerprint.Should()
            .NotBeNull();

        lastDeviceFingerprint!
            .GetMaxLength()
            .Should()
            .Be(500);

        lastDeviceFingerprint
            .IsNullable
            .Should()
            .BeTrue();

        lastLatitude.Should()
            .NotBeNull();

        lastLatitude!
            .IsNullable
            .Should()
            .BeTrue();

        lastLongitude.Should()
            .NotBeNull();

        lastLongitude!
            .IsNullable
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Verifies that UserStatus uses a string
    /// value converter and a maximum length of 50.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureStatusConversion()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var property = entityType!
            .FindProperty(nameof(UserAccount.Status));

        // Assert
        property.Should()
            .NotBeNull();

        var converter = property!
            .GetTypeMapping()
            .Converter;

        converter.Should()
            .NotBeNull();

        converter!
            .ModelClrType
            .Should()
            .Be(typeof(UserStatus));

        converter
            .ProviderClrType
            .Should()
            .Be(typeof(string));

        property
            .GetMaxLength()
            .Should()
            .Be(50);

        property
            .IsNullable
            .Should()
            .BeFalse();
    }

    /// <summary>
    /// Verifies that UserStatus.Active is converted
    /// into its string persistence representation.
    /// </summary>
    [Fact]
    public void Configure_Should_ConvertUserStatusToString()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        var property = entityType!
            .FindProperty(nameof(UserAccount.Status));

        property.Should()
            .NotBeNull();

        var converter = property!
            .GetTypeMapping()
            .Converter;

        converter.Should()
            .NotBeNull();

        // Act
        var converted =
            converter!
                .ConvertToProviderExpression
                .Compile()
                .DynamicInvoke(
                    UserStatus.Active);

        // Assert
        converted.Should()
            .Be("Active");
    }

    /// <summary>
    /// Verifies that the persisted Active string
    /// is converted into UserStatus.Active.
    /// </summary>
    [Fact]
    public void Configure_Should_ConvertStringToUserStatus()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        var property = entityType!
            .FindProperty(nameof(UserAccount.Status));

        property.Should()
            .NotBeNull();

        var converter = property!
            .GetTypeMapping()
            .Converter;

        converter.Should()
            .NotBeNull();

        // Act
        var converted =
            converter!
                .ConvertFromProviderExpression
                .Compile()
                .DynamicInvoke("Active");

        // Assert
        converted.Should()
            .Be(UserStatus.Active);
    }

    /// <summary>
    /// Verifies that Email is configured as an
    /// owned value object with the Email column,
    /// maximum length of 320, and required constraint.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureEmailOwnedValueObject()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var navigation = entityType!
            .FindNavigation(nameof(UserAccount.Email));

        // Assert
        navigation.Should()
            .NotBeNull();

        var ownedEntityType = navigation!
            .TargetEntityType;

        ownedEntityType
            .ClrType
            .Should()
            .Be(typeof(EmailAddress));

        var valueProperty = ownedEntityType
            .FindProperty("Value");

        valueProperty.Should()
            .NotBeNull();

        valueProperty!
            .GetColumnName()
            .Should()
            .Be("Email");

        valueProperty
            .GetMaxLength()
            .Should()
            .Be(320);

        valueProperty
            .IsNullable
            .Should()
            .BeFalse();
    }

    /// <summary>
    /// Verifies that Email.Value has a unique
    /// index in the owned Email entity.
    /// </summary>
    [Fact]
    public void Configure_Should_CreateUniqueIndexForEmail()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        var navigation = entityType!
            .FindNavigation(nameof(UserAccount.Email));

        navigation.Should()
            .NotBeNull();

        var ownedEntityType = navigation!
            .TargetEntityType;

        // Act
        var indexes = ownedEntityType
            .GetIndexes()
            .Where(index =>
                index.Properties.Count == 1
                && index.Properties[0].Name == "Value")
            .ToList();

        // Assert
        indexes.Should()
            .ContainSingle();

        indexes[0]
            .IsUnique
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Verifies that PhoneNumber is configured
    /// as an owned value object with the PhoneNumber
    /// column, maximum length of 30, and required constraint.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigurePhoneNumberOwnedValueObject()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var navigation = entityType!
            .FindNavigation(
                nameof(UserAccount.PhoneNumber));

        // Assert
        navigation.Should()
            .NotBeNull();

        var ownedEntityType = navigation!
            .TargetEntityType;

        ownedEntityType
            .ClrType
            .Should()
            .Be(typeof(PhoneNumber));

        var valueProperty = ownedEntityType
            .FindProperty("Value");

        valueProperty.Should()
            .NotBeNull();

        valueProperty!
            .GetColumnName()
            .Should()
            .Be("PhoneNumber");

        valueProperty
            .GetMaxLength()
            .Should()
            .Be(30);

        valueProperty
            .IsNullable
            .Should()
            .BeFalse();
    }

    /// <summary>
    /// Verifies that PhoneNumber.Value has
    /// a unique index in the owned PhoneNumber entity.
    /// </summary>
    [Fact]
    public void Configure_Should_CreateUniqueIndexForPhoneNumber()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        var navigation = entityType!
            .FindNavigation(
                nameof(UserAccount.PhoneNumber));

        navigation.Should()
            .NotBeNull();

        var ownedEntityType = navigation!
            .TargetEntityType;

        // Act
        var indexes = ownedEntityType
            .GetIndexes()
            .Where(index =>
                index.Properties.Count == 1
                && index.Properties[0].Name == "Value")
            .ToList();

        // Assert
        indexes.Should()
            .ContainSingle();

        indexes[0]
            .IsUnique
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Verifies that RoleAssignments is configured
    /// as an owned collection mapped to
    /// IdentityUserRoles.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureRoleAssignmentsOwnedCollection()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var navigation = entityType!
            .FindNavigation(
                nameof(UserAccount.RoleAssignments));

        // Assert
        navigation.Should()
            .NotBeNull();

        navigation!
            .TargetEntityType
            .ClrType
            .Should()
            .Be(typeof(RoleAssignment));

        navigation
            .TargetEntityType
            .GetTableName()
            .Should()
            .Be("IdentityUserRoles");
    }

    /// <summary>
    /// Verifies that the role assignment owned entity
    /// contains UserId as the foreign key to UserAccount.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureRoleAssignmentForeignKey()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        var navigation = entityType!
            .FindNavigation(
                nameof(UserAccount.RoleAssignments));

        navigation.Should()
            .NotBeNull();

        var ownedEntityType = navigation!
            .TargetEntityType;

        // Act
        var userIdProperty = ownedEntityType
            .FindProperty("UserId");

        var foreignKeys = ownedEntityType
            .GetForeignKeys()
            .Where(foreignKey =>
                foreignKey.Properties
                    .Any(property =>
                        property.Name == "UserId"))
            .ToList();

        // Assert
        userIdProperty.Should()
            .NotBeNull();

        userIdProperty!
            .ClrType
            .Should()
            .Be(typeof(Guid));

        foreignKeys.Should()
            .ContainSingle();

        foreignKeys[0]
            .PrincipalEntityType
            .ClrType
            .Should()
            .Be(typeof(UserAccount));
    }

    /// <summary>
    /// Verifies that RoleAssignment.RoleId is
    /// required and is never generated by the database.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureRoleAssignmentRoleId()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        var navigation = entityType!
            .FindNavigation(
                nameof(UserAccount.RoleAssignments));

        navigation.Should()
            .NotBeNull();

        var ownedEntityType = navigation!
            .TargetEntityType;

        // Act
        var property = ownedEntityType
            .FindProperty(nameof(RoleAssignment.RoleId));

        // Assert
        property.Should()
            .NotBeNull();

        property!
            .ValueGenerated
            .Should()
            .Be(ValueGenerated.Never);

        property
            .IsNullable
            .Should()
            .BeFalse();
    }

    /// <summary>
    /// Verifies that UserId and RoleId form
    /// the composite primary key of the role assignment
    /// owned entity.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureRoleAssignmentCompositeKey()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        var navigation = entityType!
            .FindNavigation(
                nameof(UserAccount.RoleAssignments));

        navigation.Should()
            .NotBeNull();

        var ownedEntityType = navigation!
            .TargetEntityType;

        // Act
        var primaryKey = ownedEntityType
            .FindPrimaryKey();

        // Assert
        primaryKey.Should()
            .NotBeNull();

        primaryKey!
            .Properties
            .Select(property => property.Name)
            .Should()
            .Equal(
                "UserId",
                nameof(RoleAssignment.RoleId));
    }

    /// <summary>
    /// Verifies that RoleAssignments uses the
    /// aggregate backing field for persistence access.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureRoleAssignmentsUsingBackingField()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var navigation = entityType!
            .FindNavigation(
                nameof(UserAccount.RoleAssignments));

        // Assert
        navigation.Should()
            .NotBeNull();

        navigation!
            .FieldInfo
            .Should()
            .NotBeNull();

        navigation
            .FieldInfo!
            .Name
            .Should()
            .Be("_roleAssignments");

        navigation
            .GetPropertyAccessMode()
            .Should()
            .Be(PropertyAccessMode.Field);
    }

    /// <summary>
    /// Verifies that DomainEvents is excluded
    /// from Entity Framework Core persistence mapping.
    /// </summary>
    [Fact]
    public void Configure_Should_IgnoreDomainEvents()
    {
        // Arrange
        var model = CreateModel();

        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        // Act
        var property = entityType!
            .FindProperty(
                nameof(UserAccount.DomainEvents));

        var navigation = entityType
            .FindNavigation(
                nameof(UserAccount.DomainEvents));

        // Assert
        property.Should()
            .BeNull();

        navigation.Should()
            .BeNull();
    }

    /// <summary>
    /// Verifies that Configure produces the
    /// complete expected persistence model for
    /// the UserAccount aggregate.
    /// </summary>
    [Fact]
    public void Configure_Should_ConfigureCompleteUserAccountPersistenceModel()
    {
        // Arrange
        var model = CreateModel();

        // Act
        var entityType = model.FindEntityType(
            typeof(UserAccount));

        entityType.Should()
            .NotBeNull();

        var emailNavigation = entityType!
            .FindNavigation(nameof(UserAccount.Email));

        var phoneNavigation = entityType
            .FindNavigation(
                nameof(UserAccount.PhoneNumber));

        var roleNavigation = entityType
            .FindNavigation(
                nameof(UserAccount.RoleAssignments));

        // Assert
        entityType
            .GetTableName()
            .Should()
            .Be("IdentityUsers");

        entityType
            .FindPrimaryKey()!
            .Properties
            .Select(property => property.Name)
            .Should()
            .Equal(nameof(UserAccount.Id));

        entityType
            .FindProperty(nameof(UserAccount.Username))!
            .GetMaxLength()
            .Should()
            .Be(100);

        entityType
            .FindProperty(nameof(UserAccount.PasswordHash))!
            .GetMaxLength()
            .Should()
            .Be(1000);

        entityType
            .FindProperty(nameof(UserAccount.SecurityStamp))!
            .GetMaxLength()
            .Should()
            .Be(100);

        entityType
            .FindProperty(nameof(UserAccount.MFAMethod))!
            .GetTypeMapping()
            .Converter
            .Should()
            .NotBeNull();

        entityType
            .FindProperty(nameof(UserAccount.Status))!
            .GetTypeMapping()
            .Converter
            .Should()
            .NotBeNull();

        emailNavigation.Should()
            .NotBeNull();

        emailNavigation!
            .TargetEntityType
            .GetTableName()
            .Should()
            .Be("IdentityUsers");

        phoneNavigation.Should()
            .NotBeNull();

        phoneNavigation!
            .TargetEntityType
            .GetTableName()
            .Should()
            .Be("IdentityUsers");

        roleNavigation.Should()
            .NotBeNull();

        roleNavigation!
            .TargetEntityType
            .GetTableName()
            .Should()
            .Be("IdentityUserRoles");

        roleNavigation
            .TargetEntityType
            .FindPrimaryKey()!
            .Properties
            .Select(property => property.Name)
            .Should()
            .Equal(
                "UserId",
                nameof(RoleAssignment.RoleId));

        entityType
            .FindProperty(
                nameof(UserAccount.DomainEvents))
            .Should()
            .BeNull();
    }
}