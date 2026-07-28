// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Users/Queries/GetUsers/GetUsersQuery.cs
// ===========================================
using Platform.Identity.Application.Contracts.Users.Dtos;
using Platform.Pipeline.Abstractions;

namespace Platform.Identity.Application.Features.Users.Queries;

    /// <summary>
    /// Represents a request to retrieve all users.
    ///
    /// Responsibility:
    /// - Trigger user listing retrieval.
    /// - Remain immutable throughout execution.
    ///
    /// Invariants:
    /// - Query contains no filtering criteria.
    /// - Future filtering should use dedicated query contracts.
    ///
    /// Side Effects:
    /// - None.
    ///
    /// Algorithm:
    /// 1. Request user collection.
    /// 2. Query use case retrieves matching users.
    /// 3. Return collection of UserDto.
    ///
    /// Complexity:
    /// O(1)
    /// </summary>
    public sealed record GetUsersQuery(): IQuery<IReadOnlyList<UserDto>>;