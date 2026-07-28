// ===========================================
// File Location :
// src/Infrastructure/Platform.Persistence/
// Configurations/UserAccountConfiguration.cs
// ===========================================
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;

namespace Platform.Persistence.Configurations;

/// <summary>
/// Configures EF Core mapping for
/// <see cref="UserAccount"/> aggregate.
///
/// Responsibility:
/// - Configure aggregate persistence.
/// - Configure owned value objects.
/// - Configure owned collections.
/// - Configure constraints.
/// - Ignore domain events.
///
/// Architectural Rules:
/// - Infrastructure mapping only.
/// - No business logic.
/// - No domain logic.
/// - No application logic.
///
/// Table:
/// IdentityUsers
///
/// Owned Types:
/// - EmailAddress
/// - PhoneNumber
///
/// Owned Collections:
/// - RoleAssignments
///
/// Domain Event Strategy:
/// - Ignore DomainEvents property.
///
/// Thread Safety:
/// - Stateless configuration.
/// </summary>
public sealed class UserAccountConfiguration
    : IEntityTypeConfiguration<UserAccount>
{
    /// <inheritdoc />
    public void Configure(
        EntityTypeBuilder<UserAccount> builder)
    {
        ArgumentNullException.ThrowIfNull(
            builder);

        ConfigureTable(builder);

        ConfigurePrimaryKey(builder);

        ConfigureProperties(builder);

        ConfigureEmail(builder);

        ConfigurePhone(builder);

        ConfigureRoleAssignments(builder);

        ConfigureIgnoredMembers(builder);
    }

    /// <summary>
    /// Configures table mapping.
    /// </summary>
    private static void ConfigureTable(
        EntityTypeBuilder<UserAccount> builder)
    {
        builder.ToTable(
            "IdentityUsers");
    }

    /// <summary>
    /// Configures primary key.
    /// </summary>
    private static void ConfigurePrimaryKey(
        EntityTypeBuilder<UserAccount> builder)
    {
        builder.HasKey(
            x => x.Id);

        builder.Property(
                x => x.Id)
            .ValueGeneratedNever();
    }

    /// <summary>
    /// Configures scalar properties.
    /// </summary>
    private static void ConfigureProperties(
        EntityTypeBuilder<UserAccount> builder)
    {
        builder.Property(
                x => x.Username)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(
                x => x.Username)
            .IsUnique();

        builder.Property(
                x => x.PasswordHash)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(
                x => x.SecurityStamp)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(
                x => x.PasswordVersion)
            .IsRequired();

        builder.Property(
                x => x.EmailVerified)
            .IsRequired();

        builder.Property(
                x => x.PhoneVerified)
            .IsRequired();

        builder.Property(
                x => x.MFAEnabled)
            .IsRequired();

        builder.Property(
                x => x.MFAMethod)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(
                x => x.TOTPSecretEncrypted)
            .HasMaxLength(2000);

        builder.Property(
                x => x.FailedLoginCount)
            .IsRequired();

        builder.Property(
                x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(
                x => x.LastLoginIp)
            .HasMaxLength(100);

        builder.Property(
                x => x.LastLoginCountry)
            .HasMaxLength(100);

        builder.Property(
                x => x.LastDeviceFingerprint)
            .HasMaxLength(500);

        builder.Property(
                x => x.LastLatitude);

        builder.Property(
                x => x.LastLongitude);

        builder.Property(
                x => x.CreatedAt)
            .IsRequired();

        builder.Property(
                x => x.UpdatedAt)
            .IsRequired();

        builder.Property(
                x => x.LastPasswordChangedAt)
            .IsRequired();
            
    }

    /// <summary>
    /// Configures EmailAddress
    /// owned value object.
    /// </summary>
    private static void ConfigureEmail(
        EntityTypeBuilder<UserAccount> builder)
    {
        builder.OwnsOne(
            x => x.Email,
            email =>
            {
                email.Property(
                        x => x.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(320)
                    .IsRequired();

                email.HasIndex(
                    x => x.Value)
                    .IsUnique();
            });
    }

    /// <summary>
    /// Configures PhoneNumber
    /// owned value object.
    /// </summary>
    private static void ConfigurePhone(
        EntityTypeBuilder<UserAccount> builder)
    {
        builder.OwnsOne(
            x => x.PhoneNumber,
            phone =>
            {
                phone.Property(
                        x => x.Value)
                    .HasColumnName("PhoneNumber")
                    .HasMaxLength(30)
                    .IsRequired();

                phone.HasIndex(
                    x => x.Value)
                    .IsUnique();
            });
    }

    /// <summary>
    /// Configures role assignments.
    /// </summary>
    private static void ConfigureRoleAssignments(
        EntityTypeBuilder<UserAccount> builder)
    {
        builder.OwnsMany(
            x => x.RoleAssignments,
            assignment =>
            {
                assignment.ToTable(
                    "IdentityUserRoles");

                assignment.WithOwner()
                    .HasForeignKey(
                        "UserId");

                assignment.Property<Guid>(
                    "UserId");

                assignment.Property(
                        x => x.RoleId)
                    .ValueGeneratedNever()
                    .IsRequired();

                assignment.HasKey(
                    "UserId",
                    nameof(RoleAssignment.RoleId));
            });

        builder.Navigation(
                x => x.RoleAssignments)
            .UsePropertyAccessMode(
                PropertyAccessMode.Field);
    }

    /// <summary>
    /// Configures ignored members.
    /// </summary>
    private static void ConfigureIgnoredMembers(
        EntityTypeBuilder<UserAccount> builder)
    {
        builder.Ignore(
            x => x.DomainEvents);
    }
}