// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Queries/GetUserByIdQuery/GetUserByIdQuery.cs
// ===========================================
using Platform.Identity.Application.Contracts.Users.Dtos;
using Platform.Pipeline.Abstractions;

namespace Platform.Identity.Application.Features.Users.Queries;

/// <summary>
/// Represents a request to retrieve a user by identifier.
///
/// Responsibility:
/// - Carry user lookup criteria.
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
/// 1. Provide UserId to query use case.
/// 2. Query use case retrieves matching user.
/// 3. Return UserDto when found.
///
/// Complexity:
/// O(1)
/// </summary>
/// <param name="UserId">
/// User identifier.
/// </param>
public sealed record GetUserByIdQuery(
    Guid UserId): IQuery<UserDto>;