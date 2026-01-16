# .NET 10 Upgrade Execution Tasks
**API Gestionnaire de Vacataire**

---

## Task Execution Dashboard

**Started**: [Not Started]  
**Current Task**: TASK-001  
**Completed**: 0 / 11  
**Failed**: 0  
**Skipped**: 0

---

## Task List

### Phase 0: Prerequisites

#### [?] TASK-001: Verify .NET 10 SDK Installation
**Description**: Confirm .NET 10 SDK is installed on development machine

**Actions**:
- [ ] (1) Run `dotnet --list-sdks` to check installed SDKs
- [ ] (2) Verify .NET 10.0.x SDK is present in the list
- [ ] (3) If missing, download from https://dotnet.microsoft.com/download/dotnet/10.0

**Verification**: 
- .NET 10 SDK appears in `dotnet --list-sdks` output

**On Failure**: 
- Install .NET 10 SDK before proceeding

---

#### [ ] TASK-002: Backup Database (Recommended)
**Description**: Create backup of Gestion_Etudiants database

**Actions**:
- [ ] (1) Confirm database `Gestion_Etudiants` exists on PT62\SQL2022
- [ ] (2) Create database backup (optional but recommended)

**Verification**: 
- Database accessible from connection string

**On Failure**: 
- Verify SQL Server 2022 instance is running
- Check connection string in appsettings.json

---

### Phase 1: Atomic Upgrade

#### [ ] TASK-003: Update Project Target Framework
**Description**: Update `API Gestionnaire de Vacataire.csproj` target framework to net10.0

**Actions**:
- [ ] (1) Open `API Gestionnaire de Vacataire.csproj`
- [ ] (2) Change `<TargetFramework>net9.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`
- [ ] (3) Save the file

**Verification**: 
- Project file contains `<TargetFramework>net10.0</TargetFramework>`

**Files Modified**: 
- API Gestionnaire de Vacataire.csproj

---

#### [ ] TASK-004: Update NuGet Packages
**Description**: Update all NuGet packages to .NET 10 compatible versions

**Actions**:
- [ ] (1) Update Microsoft.EntityFrameworkCore from 3.1.22 to 10.0.0
- [ ] (2) Update Microsoft.EntityFrameworkCore.SqlServer from 3.1.22 to 10.0.0
- [ ] (3) Update Microsoft.EntityFrameworkCore.Tools from 3.1.22 to 10.0.0
- [ ] (4) Update Newtonsoft.Json from 13.0.1 to 13.0.3
- [ ] (5) Update Swashbuckle.AspNetCore from 6.2.3 to 7.2.0
- [ ] (6) Remove Swashbuckle.Core package (deprecated)

**Package Versions**:
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.0">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="7.2.0" />
```

**Verification**: 
- All package references updated to specified versions
- Swashbuckle.Core package removed

**Files Modified**: 
- API Gestionnaire de Vacataire.csproj

---

#### [ ] TASK-005: Restore Dependencies
**Description**: Restore NuGet packages for the project

**Actions**:
- [ ] (1) Run `dotnet clean` to clean build artifacts
- [ ] (2) Run `dotnet restore` to restore packages
- [ ] (3) Verify no restore errors

**Verification**: 
- `dotnet restore` completes successfully
- No package conflict warnings

**On Failure**: 
- Review package compatibility issues
- Check for conflicting package versions

---

#### [ ] TASK-006: Build Project
**Description**: Build the project and address compilation errors

**Actions**:
- [ ] (1) Run `dotnet build --configuration Release`
- [ ] (2) Review any compilation errors or warnings
- [ ] (3) Note errors related to EF Core breaking changes

**Verification**: 
- Build completes (may have errors at this stage - expected)

**Expected Issues**:
- EF Core obsolete API warnings
- Potential LINQ query translation errors
- Missing namespace references if Swashbuckle.Core was used

**On Failure**: 
- Proceed to TASK-007 to address breaking changes

---

#### [ ] TASK-007: Address EF Core Breaking Changes
**Description**: Fix Entity Framework Core breaking changes in code

**Actions**:
- [ ] (1) Review DbContext configuration in Startup.cs or Program.cs
- [ ] (2) Update DbContext registration if needed (should remain similar)
- [ ] (3) Review Models/DbGestionnaireStagiaireContext.cs for obsolete APIs
- [ ] (4) Check Controllers for LINQ query issues
- [ ] (5) Add `.AsNoTracking()` for read-only queries (performance optimization)

**Key Changes to Review**:

**DbContext Registration** (Startup.cs or Program.cs):
```csharp
// Should work as-is, but verify:
services.AddDbContext<DbGestionnaireStagiaireContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DBGV")));
```

**Controllers** (e.g., EmploiDeTempsControllers.cs):
```csharp
// Add AsNoTracking() for GET operations:
public async Task<IActionResult> GetAll()
{
    var emplois = await _context.EmploiDeTemps
        .AsNoTracking()  // Add this
        .ToListAsync();
    return Ok(emplois);
}
```

**Verification**: 
- No obsolete API usage
- LINQ queries compatible with EF Core 10

**Files Potentially Modified**: 
- Startup.cs or Program.cs
- Models/DbGestionnaireStagiaireContext.cs
- Controllers/EmploiDeTempsControllers.cs

---

#### [ ] TASK-008: Remove Swashbuckle.Core References
**Description**: Ensure no code references to deprecated Swashbuckle.Core

**Actions**:
- [ ] (1) Review Startup.cs or Program.cs for Swashbuckle configuration
- [ ] (2) Ensure only Swashbuckle.AspNetCore is used
- [ ] (3) Verify Swagger configuration uses `AddSwaggerGen()` and `UseSwagger()`

**Expected Configuration**:
```csharp
// In Startup.cs or Program.cs
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gestionnaire de Vacataire", Version = "v1" });
});

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gestionnaire de Vacataire v1"));
```

**Verification**: 
- No `using Swashbuckle.Core` statements
- Swagger configuration uses Swashbuckle.AspNetCore

**Files Potentially Modified**: 
- Startup.cs or Program.cs

---

#### [ ] TASK-009: Rebuild and Fix Remaining Errors
**Description**: Rebuild project and address any remaining compilation errors

**Actions**:
- [ ] (1) Run `dotnet build --configuration Release`
- [ ] (2) Fix any remaining compilation errors
- [ ] (3) Address any obsolete API warnings

**Verification**: 
- `dotnet build` succeeds with 0 errors
- No critical warnings

**On Failure**: 
- Review specific error messages
- Consult EF Core 10 breaking changes documentation
- Seek guidance for unresolved errors

---

### Phase 2: Validation

#### [ ] TASK-010: Validate Database Connectivity
**Description**: Test Entity Framework Core connection to database

**Actions**:
- [ ] (1) Run `dotnet ef dbcontext info --project "API Gestionnaire de Vacataire.csproj"`
- [ ] (2) Verify connection string resolves correctly
- [ ] (3) Confirm DbContext can connect to database

**Verification**: 
- `dotnet ef dbcontext info` displays database details
- Connection to PT62\SQL2022 successful
- Database `Gestion_Etudiants` accessible

**On Failure**: 
- Verify SQL Server is running
- Check connection string in appsettings.json
- Review DbContext registration

---

#### [ ] TASK-011: Test API Endpoints via Swagger
**Description**: Validate all API endpoints function correctly

**Actions**:
- [ ] (1) Run `dotnet run --project "API Gestionnaire de Vacataire.csproj"`
- [ ] (2) Navigate to Swagger UI at https://localhost:{port}/swagger
- [ ] (3) Test each controller endpoint:
  - GET endpoints (read operations)
  - POST endpoints (create operations)  
  - PUT endpoints (update operations)
  - DELETE endpoints (delete operations)
- [ ] (4) Verify Swagger UI renders correctly
- [ ] (5) Confirm all endpoints documented and functional

**Verification**: 
- Application starts without errors
- Swagger UI page loads
- All endpoints respond with correct HTTP status codes
- JSON serialization/deserialization works
- Database operations successful

**On Failure**: 
- Review runtime errors in console
- Check controller code for LINQ query issues
- Enable EF Core logging for SQL inspection

---

### Phase 3: Finalization

#### [ ] TASK-012: Commit Changes
**Description**: Commit all upgrade changes to upgrade-to-NET10 branch

**Actions**:
- [ ] (1) Review all modified files
- [ ] (2) Stage changes: `git add .`
- [ ] (3) Commit with detailed message

**Commit Message**:
```
Upgrade to .NET 10.0

- Update target framework from net9.0 to net10.0
- Upgrade Entity Framework Core from 3.1.22 to 10.0.0
- Upgrade Swashbuckle.AspNetCore from 6.2.3 to 7.2.0
- Remove deprecated Swashbuckle.Core package
- Update Newtonsoft.Json from 13.0.1 to 13.0.3
- Address EF Core 10.0 breaking changes in DbContext
- Add AsNoTracking() for read-only queries
- Verify all API endpoints functional
- Validate Swagger UI renders correctly

Tested:
- Build successful with 0 errors/warnings
- Database connectivity verified
- All API endpoints tested via Swagger
- CRUD operations validated

Breaking changes addressed:
- EF Core query translation patterns updated
- DbContext configuration validated
- Migration compatibility verified
```

**Verification**: 
- All changes committed successfully
- Branch ready for merge to master

---

## Execution Log

_This section will be updated as tasks are executed_

### Session Started: [Timestamp will be added when execution begins]

---

## Notes

- **High Risk Area**: Entity Framework Core 3.1 ? 10.0 migration (7 major versions)
- **Rollback**: `git checkout master && git branch -D upgrade-to-NET10`
- **Database Backup**: Recommended before starting execution
- **Documentation**: plan.md contains detailed breaking changes catalog

---

## Success Criteria

? All tasks completed successfully  
? Build succeeds with 0 errors  
? Database connectivity verified  
? All API endpoints tested and functional  
? Swagger UI validated  
? Changes committed to upgrade branch
