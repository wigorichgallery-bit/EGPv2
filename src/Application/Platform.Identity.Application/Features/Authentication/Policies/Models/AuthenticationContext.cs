// ===========================================
// File Location:
//
// src/Application/Platform.Identity.Application/
// Features/Authentication/Policies/
// Models/AuthenticationContext.cs
//
// Status : LOCKED
// Repository Aligned
// ===========================================

using Platform.Identity.Application.Contracts.Authentication.Requests;
using Platform.Identity.Domain.Aggregates;

namespace Platform.Identity.Application.Features.Authentication.Policies.Models;

/// <summary>
/// Represents the immutable authentication context supplied
/// to authentication policies during login evaluation.
///
/// <para>
/// Responsibilities:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Provide the authenticated <see cref="UserAccount"/>.
/// </description>
/// </item>
/// <item>
/// <description>
/// Provide the original <see cref="LoginRequest"/>.
/// </description>
/// </item>
/// <item>
/// <description>
/// Provide the current UTC timestamp for time-based
/// authentication policy evaluation.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Architectural Rules:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Belongs to the Application layer.
/// </description>
/// </item>
/// <item>
/// <description>
/// Is immutable.
/// </description>
/// </item>
/// <item>
/// <description>
/// Contains only data required for authentication policy
/// evaluation.
/// </description>
/// </item>
/// <item>
/// <description>
/// Does not contain business logic.
/// </description>
/// </item>
/// <item>
/// <description>
/// Does not expose infrastructure dependencies.
/// </description>
/// </item>
/// </list>
///
/// <para>
/// Repository Alignment:
/// </para>
/// <list type="bullet">
/// <item>
/// <description>
/// Request context information (IP address, user agent,
/// device fingerprint, location, etc.) is intentionally
/// excluded because the current repository does not yet
/// expose an application abstraction that provides those
/// values.
/// </description>
/// </item>
/// <item>
/// <description>
/// The model can be extended in the future without changing
/// authentication policy contracts when such an abstraction
/// becomes available.
/// </description>
/// </item>
/// </list>
/// </summary>
/// <param name="User">
/// The authenticated user account.
/// </param>
/// <param name="Request">
/// The login request supplied by the caller.
/// </param>
/// <param name="CurrentUtc">
/// The current UTC timestamp.
/// </param>
public sealed record AuthenticationContext(
    UserAccount User,
    LoginRequest Request,
    DateTimeOffset CurrentUtc);