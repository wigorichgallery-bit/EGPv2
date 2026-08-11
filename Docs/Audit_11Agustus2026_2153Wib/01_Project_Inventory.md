# EGPv2 Project Inventory — Deep Inventory

Source lock: `005a340f72403600911b0631e5d099c404b484f2d8b7eddb4e38166821312fe8`

| Project | Layer | Files | Lines | Types | Classes | Interfaces | Records | Enums | Empty .cs | Test methods |

|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| `Platform.Identity.Application` | Application | 103 | 8122 | 98 | 38 | 26 | 30 | 4 | 3 | 0 |
| `Platform.Pipeline` | Application | 30 | 1871 | 29 | 11 | 17 | 1 | 0 | 0 | 0 |
| `Platform.Security.Application` | Application | 1 | 6 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `Platform.Identity.Domain` | Core | 35 | 4112 | 34 | 28 | 0 | 0 | 6 | 0 | 0 |
| `Platform.Security.Domain` | Core | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `Platform.SharedKernel` | Core | 16 | 986 | 14 | 10 | 3 | 0 | 1 | 0 | 0 |
| `Platform.Communication` | Infrastructure | 97 | 5645 | 95 | 55 | 27 | 10 | 3 | 0 | 0 |
| `Platform.Persistence` | Infrastructure | 21 | 3210 | 16 | 16 | 0 | 0 | 0 | 4 | 0 |
| `Platform.Security.Infrastructure` | Infrastructure | 14 | 1317 | 13 | 13 | 0 | 0 | 0 | 0 | 0 |
| `Platform.TokenProvider` | Infrastructure | 6 | 74 | 1 | 1 | 0 | 0 | 0 | 4 | 0 |
| `Platform.WebApi` | Web | 26 | 3962 | 24 | 23 | 0 | 1 | 0 | 0 | 0 |
| `Platform.Communication.UnitTests` | tests | 60 | 13118 | 56 | 56 | 0 | 0 | 0 | 4 | 377 |
| `Platform.Identity.Application.UnitTests` | tests | 76 | 16486 | 75 | 75 | 0 | 0 | 0 | 0 | 442 |
| `Platform.Identity.Domain.UnitTests` | tests | 35 | 10460 | 34 | 34 | 0 | 0 | 0 | 0 | 395 |
| `Platform.Persistence.UnitTests` | tests | 12 | 6090 | 11 | 11 | 0 | 0 | 0 | 0 | 157 |
| `Platform.Pipeline.UnitTests` | tests | 29 | 2987 | 16 | 12 | 0 | 4 | 0 | 16 | 89 |
| `Platform.Security.Infrastructure.UnitTests` | tests | 17 | 3225 | 16 | 16 | 0 | 0 | 0 | 0 | 101 |
| `Platform.SharedKernel.UnitTests` | tests | 14 | 3961 | 21 | 20 | 0 | 0 | 1 | 0 | 98 |
| `Platform.TokenProvider.UnitTests` | tests | 2 | 97 | 1 | 1 | 0 | 0 | 0 | 0 | 3 |

## Project-by-project dependency inventory

### Platform.Identity.Application
- Path: `src/Application/Platform.Identity.Application/Platform.Identity.Application.csproj`
- Target: `net10.0`
- Project references: `..\..\Core\Platform.SharedKernel\Platform.SharedKernel.csproj`, `..\..\Core\Platform.Identity.Domain\Platform.Identity.Domain.csproj`, `..\Platform.Pipeline\Platform.Pipeline.csproj`
- Package references: `FluentValidation 12.1.1`, `Microsoft.Extensions.Logging.Abstractions 10.0.10`, `Microsoft.Extensions.Options 10.0.10`
- Inventory: 103 files, 8122 lines, 98 type declarations.
- Zero-byte files:
  - `src/Application/Platform.Identity.Application/Contracts/Authentications/Responses/VerifyTotpResponse.cs`
  - `src/Application/Platform.Identity.Application/Contracts/Authentications/Requests/VerifyTotpRequest.cs`
  - `src/Application/Platform.Identity.Application/Contracts/Authentications/Dtos/AuthenticationChallengeDto.cs`

### Platform.Pipeline
- Path: `src/Application/Platform.Pipeline/Platform.Pipeline.csproj`
- Target: `net10.0`
- Project references: `..\..\Core\Platform.SharedKernel\Platform.SharedKernel.csproj`, `..\Platform.Security.Application\Platform.Security.Application.csproj`
- Package references: none
- Inventory: 30 files, 1871 lines, 29 type declarations.

### Platform.Security.Application
- Path: `src/Application/Platform.Security.Application/Platform.Security.Application.csproj`
- Target: `net10.0`
- Project references: `..\..\Core\Platform.SharedKernel\Platform.SharedKernel.csproj`, `..\..\Core\Platform.Security.Domain\Platform.Security.Domain.csproj`, `..\..\Core\Platform.Identity.Domain\Platform.Identity.Domain.csproj`
- Package references: none
- Inventory: 1 files, 6 lines, 0 type declarations.

### Platform.Identity.Domain
- Path: `src/Core/Platform.Identity.Domain/Platform.Identity.Domain.csproj`
- Target: `net10.0`
- Project references: `..\Platform.SharedKernel\Platform.SharedKernel.csproj`
- Package references: none
- Inventory: 35 files, 4112 lines, 34 type declarations.

### Platform.Security.Domain
- Path: `src/Core/Platform.Security.Domain/Platform.Security.Domain.csproj`
- Target: `net10.0`
- Project references: `..\Platform.SharedKernel\Platform.SharedKernel.csproj`
- Package references: none
- Inventory: 0 files, 0 lines, 0 type declarations.

### Platform.SharedKernel
- Path: `src/Core/Platform.SharedKernel/Platform.SharedKernel.csproj`
- Target: `net10.0`
- Project references: none
- Package references: none
- Inventory: 16 files, 986 lines, 14 type declarations.

### Platform.Communication
- Path: `src/Infrastructure/Platform.Communication/Platform.Communication.csproj`
- Target: `net10.0`
- Project references: `..\..\Core\Platform.SharedKernel\Platform.SharedKernel.csproj`
- Package references: `Azure.Identity 1.21.0`, `Infobip.Api.SDK 1.0.3`, `MailKit 4.17.0`, `Microsoft.Extensions.Configuration 10.0.10`, `Microsoft.Extensions.DependencyInjection 10.0.10`, `Microsoft.Extensions.Options.ConfigurationExtensions 10.0.10`, `Microsoft.Graph 6.2.0`, `SendGrid 9.29.3`, `Twilio 7.14.9`, `Vonage 8.35.0`
- Inventory: 97 files, 5645 lines, 95 type declarations.

### Platform.Persistence
- Path: `src/Infrastructure/Platform.Persistence/Platform.Persistence.csproj`
- Target: `net10.0`
- Project references: `..\..\Core\Platform.SharedKernel\Platform.SharedKernel.csproj`, `..\..\Core\Platform.Identity.Domain\Platform.Identity.Domain.csproj`, `..\..\Application\Platform.Identity.Application\Platform.Identity.Application.csproj`
- Package references: `Microsoft.EntityFrameworkCore 10.0.9`, `Microsoft.EntityFrameworkCore.Design 10.0.9`, `Microsoft.EntityFrameworkCore.Relational 10.0.9`, `Microsoft.EntityFrameworkCore.SqlServer 10.0.9`
- Inventory: 21 files, 3210 lines, 16 type declarations.
- Zero-byte files:
  - `src/Infrastructure/Platform.Persistence/Projections/PermissionProjection.cs`
  - `src/Infrastructure/Platform.Persistence/Projections/GovernanceProjection.cs`
  - `src/Infrastructure/Platform.Persistence/Projections/ApprovalProjection.cs`
  - `src/Infrastructure/Platform.Persistence/Projections/AuditProjection.cs`

### Platform.Security.Infrastructure
- Path: `src/Infrastructure/Platform.Security.Infrastructure/Platform.Security.Infrastructure.csproj`
- Target: `net10.0`
- Project references: `..\..\Core\Platform.SharedKernel\Platform.SharedKernel.csproj`, `..\..\Application\Platform.Identity.Application\Platform.Identity.Application.csproj`
- Package references: `Microsoft.Extensions.Options 10.0.10`
- Inventory: 14 files, 1317 lines, 13 type declarations.

### Platform.TokenProvider
- Path: `src/Infrastructure/Platform.TokenProvider/Platform.TokenProvider.csproj`
- Target: `net10.0`
- Project references: `..\..\Core\Platform.SharedKernel\Platform.SharedKernel.csproj`, `..\..\Application\Platform.Identity.Application\Platform.Identity.Application.csproj`
- Package references: `Microsoft.AspNetCore.Authentication.JwtBearer 10.0.9`, `System.IdentityModel.Tokens.Jwt 8.19.1`
- Inventory: 6 files, 74 lines, 1 type declarations.
- Zero-byte files:
  - `src/Infrastructure/Platform.TokenProvider/DependencyInjection/TokenProviderServiceCollectionExtensions.cs`
  - `src/Infrastructure/Platform.TokenProvider/Jwt/JwtBearerEventsHandler.cs`
  - `src/Infrastructure/Platform.TokenProvider/Jwt/JwtTokenProvider.cs`
  - `src/Infrastructure/Platform.TokenProvider/Jwt/JwtClaimsFactory.cs`

### Platform.WebApi
- Path: `src/Web/Platform.WebApi/Platform.WebApi.csproj`
- Target: `net10.0`
- Project references: `..\..\Application\Platform.Identity.Application\Platform.Identity.Application.csproj`, `..\..\Application\Platform.Security.Application\Platform.Security.Application.csproj`, `..\..\Application\Platform.Pipeline\Platform.Pipeline.csproj`, `..\..\Infrastructure\Platform.Persistence\Platform.Persistence.csproj`, `..\..\Infrastructure\Platform.Security.Infrastructure\Platform.Security.Infrastructure.csproj`, `..\..\Infrastructure\Platform.TokenProvider\Platform.TokenProvider.csproj`
- Package references: `Microsoft.AspNetCore.OpenApi 10.0.7`, `Microsoft.EntityFrameworkCore.Design 10.0.9`, `Swashbuckle.AspNetCore 10.2.3`
- Inventory: 26 files, 3962 lines, 24 type declarations.

### Platform.Communication.UnitTests
- Path: `tests/Platform.Communication.UnitTests/Platform.Communication.UnitTests.csproj`
- Target: `net10.0`
- Project references: `..\..\src\Infrastructure\Platform.Communication\Platform.Communication.csproj`
- Package references: `coverlet.collector 10.0.1`, `FluentAssertions 8.10.0`, `Microsoft.NET.Test.Sdk 18.8.1`, `NSubstitute 6.0.0`, `xunit 2.9.3`, `xunit.runner.visualstudio 3.1.5`
- Inventory: 60 files, 13118 lines, 56 type declarations.
- Zero-byte files:
  - `tests/Platform.Communication.UnitTests/Channels/WhatsApp/Clients/MetaCloudClientTests.cs`
  - `tests/Platform.Communication.UnitTests/Channels/WhatsApp/Clients/TwilioWhatsAppClientTests.cs`
  - `tests/Platform.Communication.UnitTests/Channels/Sms/Clients/TwilioSmsClientTests.cs`
  - `tests/Platform.Communication.UnitTests/Channels/Sms/Clients/VonageSmsClientTests.cs`

### Platform.Identity.Application.UnitTests
- Path: `tests/Platform.Identity.Application.UnitTests/Platform.Identity.Application.UnitTests.csproj`
- Target: `net10.0`
- Project references: `..\..\src\Application\Platform.Identity.Application\Platform.Identity.Application.csproj`, `..\..\src\Core\Platform.Identity.Domain\Platform.Identity.Domain.csproj`, `..\..\src\Core\Platform.SharedKernel\Platform.SharedKernel.csproj`, `..\..\src\Application\Platform.Pipeline\Platform.Pipeline.csproj`
- Package references: `coverlet.collector 10.0.1`, `FluentAssertions 8.10.0`, `Microsoft.NET.Test.Sdk 18.8.1`, `Moq 4.20.72`, `xunit 2.9.3`, `xunit.runner.visualstudio 3.1.5`
- Inventory: 76 files, 16486 lines, 75 type declarations.

### Platform.Identity.Domain.UnitTests
- Path: `tests/Platform.Identity.Domain.UnitTests/Platform.Identity.Domain.UnitTests.csproj`
- Target: `net10.0`
- Project references: `..\..\src\Core\Platform.Identity.Domain\Platform.Identity.Domain.csproj`, `..\..\src\Core\Platform.SharedKernel\Platform.SharedKernel.csproj`
- Package references: `coverlet.collector 10.0.1`, `FluentAssertions 8.10.0`, `Microsoft.NET.Test.Sdk 18.8.1`, `xunit 2.9.3`, `xunit.runner.visualstudio 3.1.5`
- Inventory: 35 files, 10460 lines, 34 type declarations.

### Platform.Persistence.UnitTests
- Path: `tests/Platform.Persistence.UnitTests/Platform.Persistence.UnitTests.csproj`
- Target: `net10.0`
- Project references: `..\..\src\Infrastructure\Platform.Persistence\Platform.Persistence.csproj`
- Package references: `coverlet.collector 10.0.1`, `FluentAssertions 8.10.0`, `Microsoft.Data.Sqlite 10.0.9`, `Microsoft.EntityFrameworkCore.InMemory 10.0.9`, `Microsoft.EntityFrameworkCore.Sqlite 10.0.9`, `Microsoft.NET.Test.Sdk 18.8.1`, `NSubstitute 6.0.0`, `xunit 2.9.3`, `xunit.runner.visualstudio 3.1.5`
- Inventory: 12 files, 6090 lines, 11 type declarations.

### Platform.Pipeline.UnitTests
- Path: `tests/Platform.Pipeline.UnitTests/Platform.Pipeline.UnitTests.csproj`
- Target: `net10.0`
- Project references: `..\..\src\Application\Platform.Pipeline\Platform.Pipeline.csproj`, `..\..\src\Core\Platform.SharedKernel\Platform.SharedKernel.csproj`
- Package references: `coverlet.collector 10.0.1`, `FluentAssertions 8.10.0`, `Microsoft.NET.Test.Sdk 18.8.1`, `Moq 4.20.72`, `xunit 2.9.3`, `xunit.runner.visualstudio 3.1.5`
- Inventory: 29 files, 2987 lines, 16 type declarations.
- Zero-byte files:
  - `tests/Platform.Pipeline.UnitTests/Abstractions/IExecutionLoggerTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/IPipelineExecutorTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/IGovernanceEvaluatorTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/IQueryTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/IValidatorTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/ICommandTTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/ICommandHandlerTTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/IPipelineBehaviorTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/IQueryValidatorTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/IPipelineOrderedTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/ICommandHandlerTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/IQueryHandlerTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/IQueryTTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/ICommandValidatorTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/IGovernanceRequestTests.cs`
  - `tests/Platform.Pipeline.UnitTests/Abstractions/ICommandTests.cs`

### Platform.Security.Infrastructure.UnitTests
- Path: `tests/Platform.Security.Infrastructure.UnitTests/Platform.Security.Infrastructure.UnitTests.csproj`
- Target: `net10.0`
- Project references: `..\..\src\Infrastructure\Platform.Security.Infrastructure\Platform.Security.Infrastructure.csproj`, `..\..\src\Application\Platform.Identity.Application\Platform.Identity.Application.csproj`, `..\..\src\Core\Platform.Identity.Domain\Platform.Identity.Domain.csproj`, `..\..\src\Core\Platform.SharedKernel\Platform.SharedKernel.csproj`, `..\..\src\Application\Platform.Pipeline\Platform.Pipeline.csproj`
- Package references: `coverlet.collector 10.0.1`, `FluentAssertions 8.10.0`, `Microsoft.NET.Test.Sdk 18.8.1`, `Moq 4.20.72`, `xunit 2.9.3`, `xunit.runner.visualstudio 3.1.5`
- Inventory: 17 files, 3225 lines, 16 type declarations.

### Platform.SharedKernel.UnitTests
- Path: `tests/Platform.SharedKernel.UnitTests/Platform.SharedKernel.UnitTests.csproj`
- Target: `net10.0`
- Project references: `..\..\src\Core\Platform.SharedKernel\Platform.SharedKernel.csproj`
- Package references: `coverlet.collector 10.0.1`, `FluentAssertions 8.10.0`, `Microsoft.NET.Test.Sdk 18.8.1`, `Moq 4.20.72`, `xunit 2.9.3`, `xunit.runner.visualstudio 3.1.5`
- Inventory: 14 files, 3961 lines, 21 type declarations.

### Platform.TokenProvider.UnitTests
- Path: `tests/Platform.TokenProvider.UnitTests/Platform.TokenProvider.UnitTests.csproj`
- Target: `net10.0`
- Project references: `..\..\src\Infrastructure\Platform.TokenProvider\Platform.TokenProvider.csproj`, `..\..\src\Application\Platform.Identity.Application\Platform.Identity.Application.csproj`, `..\..\src\Application\Platform.Security.Application\Platform.Security.Application.csproj`, `..\..\src\Core\Platform.SharedKernel\Platform.SharedKernel.csproj`
- Package references: `coverlet.collector 10.0.1`, `FluentAssertions 8.10.0`, `Microsoft.NET.Test.Sdk 18.8.1`, `NSubstitute 6.0.0`, `xunit 2.9.3`, `xunit.runner.visualstudio 3.1.5`
- Inventory: 2 files, 97 lines, 1 type declarations.
