
// ===========================================
// File Location :
// src/Infrastructure/Platform.Persistence/
// Configurations/AuthenticationChallengeConfiguration.cs
// ===========================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Platform.Identity.Domain.Aggregates;
using Platform.Identity.Domain.ValueObjects;

namespace Platform.Persistence.Configurations;

/// <summary>
/// Configures persistence mapping for the
/// <see cref="AuthenticationChallenge"/> aggregate.
///
/// <para>
/// Responsibility:
/// - Configure authentication challenge persistence.
/// - Map aggregate scalar properties.
/// - Map the <see cref="ChallengeSecret"/> value object.
/// - Configure primary key and required fields.
/// - Prevent domain events from being persisted.
/// </para>
///
/// <para>
/// Architectural Rules:
/// - Persistence configuration only.
/// - No business logic.
/// - No domain state mutation.
/// - No application orchestration.
/// - No infrastructure service resolution.
/// </para>
/// </summary>
public sealed class AuthenticationChallengeConfiguration
    : IEntityTypeConfiguration<AuthenticationChallenge>
{
    /// <summary>
    /// Configures the
    /// <see cref="AuthenticationChallenge"/> entity.
    /// </summary>
    /// <param name="builder">
    /// Entity type builder.
    /// </param>
    public void Configure(
        EntityTypeBuilder<AuthenticationChallenge> builder)
    {
        ArgumentNullException.ThrowIfNull(
            builder);

        // ===========================================
        // Table
        // ===========================================

        builder.ToTable(
            "AuthenticationChallenges");

        // ===========================================
        // Primary Key
        // ===========================================

        builder.HasKey(
            x => x.Id);

        builder.Property(
            x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        // ===========================================
        // User
        // ===========================================

        builder.Property(
            x => x.UserId)
            .IsRequired();

        // ===========================================
        // Challenge Configuration
        // ===========================================

        builder.Property(
            x => x.ChallengeType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(
            x => x.Purpose)
            .HasConversion<string>()
            .IsRequired();

        // ===========================================
        // Challenge Secret
        // ===========================================
        //
        // ChallengeSecret is a single-property
        // value object containing the protected
        // challenge secret.
        //
        // Persist only the underlying Value.
        // The domain value object remains responsible
        // for protecting its internal representation.
        // ===========================================

        builder.Property(
            x => x.ChallengeSecret)
            .HasConversion(
                secret => secret.Value,
                value => new ChallengeSecret(value))
            .HasColumnName(
                "ChallengeSecret")
            .IsRequired();

        // ===========================================
        // Lifecycle
        // ===========================================

        builder.Property(
            x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(
            x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(
            x => x.ExpiresAtUtc)
            .IsRequired();

        builder.Property(
            x => x.CompletedAtUtc)
            .IsRequired(false);

        builder.Property(
            x => x.CancellationReason)
            .HasConversion<string>()
            .IsRequired(false);

        builder.Property(
            x => x.CancelledAtUtc)
            .IsRequired(false);

        builder.Property(
            x => x.LockedAtUtc)
            .IsRequired(false);

        // ===========================================
        // Retry
        // ===========================================

        builder.Property(
            x => x.FailedAttemptCount)
            .IsRequired();

        // ===========================================
        // Domain Events
        // ===========================================
        //
        // DomainEvents belongs to the aggregate root
        // infrastructure boundary and must never be
        // persisted as entity state.
        // ===========================================

        builder.Ignore(
            x => x.DomainEvents);
    }
}
