// ===========================================
// File Location : src/Application/Platform.Identity.Application/Features/Roles/Queries/GetRoles/GetRolesQuery.cs
// ===========================================
using Platform.Identity.Application.Contracts.Roles.Dtos;
using Platform.Pipeline.Abstractions;

namespace Platform.Identity.Application.Features.Roles.Queries;

/// <summary>
/// Represents a request to retrieve available roles.
///
/// Responsibility:
/// - Trigger role listing retrieval.
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
/// 1. Request role collection.
/// 2. Query use case retrieves matching roles.
/// 3. Return collection of RoleDto.
///
/// Complexity:
/// O(1)
/// </summary>
public sealed record GetRolesQuery(): IQuery<IReadOnlyList<RoleDto>>;