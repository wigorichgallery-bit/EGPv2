// ===========================================
// File Location :
// src/Infrastructure/Platform.Persistence/
// Configurations/RoleConfiguration.cs
// ===========================================
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;

namespace Platform.Persistence.Configurations;

/// <summary>
/// Configures Entity Framework Core persistence
/// mapping for the <see cref="Role"/> aggregate.
///
/// Responsibility:
/// - Configure aggregate persistence.
/// - Configure primary key.
/// - Configure scalar properties.
/// - Configure value object conversions.
/// - Configure owned collections.
/// - Configure backing field mapping.
/// - Ignore non-persistent members.
///
/// Architectural Rules:
/// - Infrastructure layer only.
/// - No business logic.
/// - No domain decision making.
/// - No application orchestration.
/// - Persistence mapping only.
///
/// Persistence Strategy:
/// - Aggregate Root.
/// - ValueConverter for single-property
///   value objects.
/// - Owned collection for permission
///   identifiers.
/// - Backing field access for collections.
///
/// Table:
/// IdentityRoles
///
/// Related Tables:
/// - IdentityRolePermissions
///
/// Thread Safety:
/// - Stateless.
/// - Singleton safe.
///
/// EF Core Compatibility:
/// - EF Core 10.
/// </summary>
public sealed class RoleConfiguration
    : IEntityTypeConfiguration<Role>
{
    /// <summary>
    /// Configures persistence mapping
    /// for <see cref="Role"/>.
    ///
    /// Algorithm:
    /// 1. Configure table.
    /// 2. Configure primary key.
    /// 3. Configure scalar properties.
    /// 4. Configure value objects.
    /// 5. Configure owned collections.
    /// 6. Configure ignored members.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="builder">
    /// Entity type builder.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when
    /// <paramref name="builder"/>
    /// is null.
    /// </exception>
    public void Configure(
        EntityTypeBuilder<Role> builder)
    {
        ArgumentNullException.ThrowIfNull(
            builder);

        ConfigureTable(
            builder);

        ConfigurePrimaryKey(
            builder);

        ConfigureProperties(
            builder);

        ConfigureRoleScope(
            builder);

        ConfigurePermissions(
            builder);

        ConfigureIgnoredMembers(
            builder);
    }

    /// <summary>
    /// Configures table mapping.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="builder">
    /// Entity builder.
    /// </param>
    private static void ConfigureTable(
        EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(
            "IdentityRoles");
    }

    /// <summary>
    /// Configures aggregate primary key.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="builder">
    /// Entity builder.
    /// </param>
    private static void ConfigurePrimaryKey(
        EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(
            x => x.Id);

        builder.Property(
                x => x.Id)
            .ValueGeneratedNever();
    }

    /// <summary>
    /// Configures scalar properties.
    ///
    /// Responsibility:
    /// - Configure required fields.
    /// - Configure maximum lengths.
    /// - Configure indexes.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="builder">
    /// Entity builder.
    /// </param>
    private static void ConfigureProperties(
        EntityTypeBuilder<Role> builder)
    {
        builder.Property(
                x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(
                x => x.Name)
            .IsUnique();

        builder.Property(
                x => x.IsSystemRole)
            .IsRequired();

        builder.Property(
                x => x.IsActive)
            .IsRequired();

        builder.Property(
                x => x.CreatedAt)
            .IsRequired();
    }

    /// <summary>
    /// Configures persistence mapping for
    /// <see cref="RoleScope"/>.
    ///
    /// Responsibility:
    /// - Persist RoleScope as a scalar column.
    /// - Convert between RoleScope and string.
    /// - Keep domain model free from
    ///   persistence concerns.
    ///
    /// Persistence Strategy:
    /// - ValueConverter.
    /// - Single column.
    ///
    /// Database Column:
    /// Scope
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="builder">
    /// Entity builder.
    /// </param>
    private static void ConfigureRoleScope(
        EntityTypeBuilder<Role> builder)
    {
        ArgumentNullException.ThrowIfNull(
            builder);

        var converter =
            new ValueConverter<RoleScope, string>(
                scope => scope.Value,
                value => RoleScope.From(value));

        builder.Property(
                x => x.Scope)
            .HasConversion(
                converter)
            .HasColumnName(
                "Scope")
            .HasMaxLength(100)
            .IsRequired();
    }

    /// <summary>
    /// Configures persistence mapping for
    /// role permission identifiers.
    ///
    /// Responsibility:
    /// - Persist permission collection.
    /// - Configure owned value objects.
    /// - Configure composite primary key.
    /// - Configure backing field access.
    ///
    /// Persistence Strategy:
    /// - OwnsMany.
    /// - Composite key.
    /// - No surrogate key.
    ///
    /// Database Table:
    /// IdentityRolePermissions
    ///
    /// Primary Key:
    /// - RoleId
    /// - PermissionCode
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="builder">
    /// Entity builder.
    /// </param>
    private static void ConfigurePermissions(
        EntityTypeBuilder<Role> builder)
    {
        ArgumentNullException.ThrowIfNull(
            builder);

        builder.OwnsMany(
            x => x.PermissionIds,
            permission =>
            {
                permission.ToTable(
                    "IdentityRolePermissions");

                permission.WithOwner()
                    .HasForeignKey(
                        "RoleId");

                permission.Property<Guid>(
                    "RoleId");

                permission.Property(
                        x => x.Value)
                    .HasColumnName(
                        "PermissionCode")
                    .HasMaxLength(200)
                    .IsRequired();

                permission.HasKey(
                    "RoleId",
                    "Value");
            });

        builder.Navigation(
                x => x.PermissionIds)
            .UsePropertyAccessMode(
                PropertyAccessMode.Field);
    }

    /// <summary>
    /// Configures ignored aggregate members.
    ///
    /// Responsibility:
    /// - Ignore non-persistent members.
    /// - Configure backing field metadata.
    /// - Finalize aggregate mapping.
    ///
    /// Architectural Rules:
    /// - Ignore domain events.
    /// - Preserve aggregate encapsulation.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    /// <param name="builder">
    /// Entity builder.
    /// </param>
    private static void ConfigureIgnoredMembers(
        EntityTypeBuilder<Role> builder)
    {
        ArgumentNullException.ThrowIfNull(
            builder);

        builder.Ignore(
            x => x.DomainEvents);

        builder.Metadata
            .FindNavigation(
                nameof(Role.PermissionIds))
            !
            .SetField(
                "_permissionIds");

        builder.Metadata
            .FindNavigation(
                nameof(Role.PermissionIds))
            !
            .SetPropertyAccessMode(
                PropertyAccessMode.Field);
    }
}