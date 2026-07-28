// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Queries/GetUserByUsername/GetUserByUsernameQuery.cs
// ===========================================
using Platform.Identity.Application.Contracts.Users.Dtos;
using Platform.Pipeline.Abstractions;

namespace Platform.Identity.Application.Features.Users.Queries;

/// <summary>
/// Represents a request to retrieve a user by username.
///
/// Responsibility:
/// - Carry username lookup criteria.
/// - Remain immutable throughout execution.
///
/// Invariants:
/// - Query does not perform validation.
/// - Validation belongs to use case if required.
///
/// Side Effects:
/// - None.
///
/// Algorithm:
/// 1. Provide Username to query use case.
/// 2. Query use case retrieves matching user.
/// 3. Return UserDto when found.
///
/// Complexity:
/// O(1)
/// </summary>
/// <param name="Username">
/// Username to search.
/// </param>
public sealed record GetUserByUsernameQuery(
    string Username): IQuery<UserDto>;