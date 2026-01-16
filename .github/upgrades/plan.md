# .NET 10 Upgrade Migration Plan
**API Gestionnaire de Vacataire**

---

## Table of Contents
1. [Executive Summary](#1-executive-summary)
2. [Migration Strategy](#2-migration-strategy)
3. [Detailed Dependency Analysis](#3-detailed-dependency-analysis)
4. [Project-by-Project Plans](#4-project-by-project-plans)
5. [Risk Management](#5-risk-management)
6. [Testing & Validation Strategy](#6-testing--validation-strategy)
7. [Complexity & Effort Assessment](#7-complexity--effort-assessment)
8. [Source Control Strategy](#8-source-control-strategy)
9. [Success Criteria](#9-success-criteria)

---

## 1. Executive Summary

### Scenario Description
Upgrade **API Gestionnaire de Vacataire** from **.NET 9.0** to **.NET 10.0 LTS**.

### Scope
**Projects Affected**: 1 project
- API Gestionnaire de Vacataire (ASP.NET Core Web API)

**Current State**:
- Target Framework: `net9.0`
- Entity Framework Core: `3.1.22` (Critical: 7 major versions behind)
- Swashbuckle.AspNetCore: `6.2.3`
- Newtonsoft.Json: `13.0.1`

**Target State**:
- Target Framework: `net10.0`
- Entity Framework Core: `10.0.x` (aligned with .NET 10)
- Swashbuckle.AspNetCore: `7.2.0` or higher
- All packages updated to .NET 10 compatible versions

### Selected Strategy
**All-At-Once Strategy** - Single atomic upgrade operation.

**Rationale**: 
- Single project solution (simplest structure)
- All updates can be coordinated in one pass
- Fastest path to completion
- No intermediate multi-targeting complexity

### Discovered Metrics
- **Projects**: 1
- **Current LOC**: ~Medium (ASP.NET Core API with EF Core)
- **Package Count**: 6 NuGet packages
- **Dependency Depth**: 0 (no project dependencies)

### Complexity Classification
**Simple Solution Structure with High Migration Complexity**

**Justification**:
- ? Single project (no dependency coordination needed)
- ? Clean ASP.NET Core Web API structure
- ?? **Critical Issue**: Entity Framework Core 3.1.22 ? 10.0.x represents **7 major version jumps**
- ?? EF Core 6.0, 7.0, 8.0, 9.0, and 10.0 each introduced breaking changes
- ?? Database context, migrations, and query patterns may require significant updates

### Critical Issues
1. **Entity Framework Core Major Version Gap** (High Risk)
   - Current: 3.1.22 (released 2021, end-of-life)
   - Target: 10.0.x (current LTS)
   - Breaking changes across 7 major versions include:
     - DbContext configuration changes
     - Query translation improvements (potential behavior changes)
     - Migration system updates
     - SQL Server provider changes
     - Tracking behavior modifications

2. **Package Compatibility**
   - Swashbuckle.Core 5.6.0 is deprecated and incompatible with .NET 10
   - All packages require version updates

### Expected Remaining Iterations
- Iteration 2.1: Dependency Analysis (N/A - single project)
- Iteration 2.2: Migration Strategy Details
- Iteration 2.3: Project Details & Risk Assessment
- Iteration 3.1: Complete Project Migration Specifications
- **Total**: 4 additional iterations

## 2. Migration Strategy

### Approach Selection
**All-At-Once Strategy** - Single atomic upgrade operation

### Justification
**Why All-At-Once is Optimal**:
1. **Single Project**: No coordination between multiple projects needed
2. **No Intermediate States**: Framework and packages updated together
3. **Fastest Completion**: All changes in one coordinated pass
4. **Simplified Testing**: Single test cycle after all updates complete
5. **Clean Dependency Resolution**: All packages resolve against net10.0 simultaneously

**Why Incremental is Not Needed**:
- No dependency ordering concerns (single project)
- No need for multi-targeting complexity
- Testing can be comprehensive in one pass
- No risk of partial upgrades leaving solution in broken state

### All-At-Once Strategy Rationale
The atomic approach is ideal for this scenario:
- **Single Buildable Unit**: One project = one build verification
- **Package Coherence**: EF Core packages must be upgraded together (3.1.22 ? 10.0.x)
- **Risk Containment**: If issues arise, entire upgrade can be reverted as one unit
- **Team Efficiency**: No context switching between migration phases

### Dependency-Based Ordering
**N/A** - Single project has no internal dependencies. External package dependencies will be updated simultaneously.

### Execution Approach
**Sequential Steps in Single Operation**:
1. Update `TargetFramework` property to `net10.0`
2. Update all package references to .NET 10 compatible versions
3. Remove deprecated `Swashbuckle.Core` package
4. Restore dependencies
5. Build and address compilation errors
6. Fix Entity Framework Core breaking changes
7. Validate database connectivity
8. Test API functionality

### Timeline

#### Phase 0: Preparation
**Already Complete**:
- ? Pending changes committed
- ? Upgrade branch created (`upgrade-to-NET10`)
- ? Assessment completed

#### Phase 1: Atomic Upgrade
**Operations** (performed as single coordinated batch):
- Update project file target framework
- Update all package references
- Remove deprecated packages
- Restore dependencies
- Build solution and fix all compilation errors
- Address EF Core breaking changes
- Validate database connectivity

**Deliverables**: Project builds with 0 errors, database connects successfully

#### Phase 2: Validation
**Operations**:
- Test API endpoints
- Verify database queries
- Validate Swagger UI functionality
- Performance baseline check

**Deliverables**: All API endpoints respond correctly, Swagger documentation renders

### Parallel vs Sequential
**N/A** - Single project executes sequentially through migration steps.

### Risk Management During Migration
- **EF Core Breaking Changes**: Most significant risk - detailed catalog provided in §5
- **Database Compatibility**: SQL Server 2022 fully supports .NET 10
- **API Contract Preservation**: Controller signatures should remain unchanged
- **Configuration Migration**: Minimal - appsettings.json format unchanged

### Rollback Strategy
**Single Commit Approach**:
- All changes committed as one atomic unit
- Rollback via `git revert` or branch deletion
- No intermediate commits to manage
- Database migrations can be reverted if needed

## 3. Detailed Dependency Analysis

### Project Structure
**Single Project Solution** - No internal project dependencies

```
API Gestionnaire de Vacataire (ASP.NET Core Web API)
??? No project dependencies
??? 6 NuGet package dependencies
```

### Dependency Graph Summary
This is a standalone ASP.NET Core Web API project with no dependency on other projects in the solution. The migration is straightforward from a structural perspective but complex from a package compatibility standpoint.

### Project Grouping
**Single Migration Phase** - All changes occur atomically in one operation

**Phase 1: Atomic Upgrade**
- API Gestionnaire de Vacataire.csproj

### Critical Path
Since there's only one project, the critical path is linear:
1. Update target framework to `net10.0`
2. Update all NuGet packages to .NET 10 compatible versions
3. Address Entity Framework Core breaking changes
4. Fix compilation errors
5. Validate database connectivity and queries
6. Test API endpoints

### Package Dependencies
**Current Package References**:
1. Microsoft.EntityFrameworkCore: `3.1.22`
2. Microsoft.EntityFrameworkCore.SqlServer: `3.1.22`
3. Microsoft.EntityFrameworkCore.Tools: `3.1.22`
4. Newtonsoft.Json: `13.0.1`
5. Swashbuckle.AspNetCore: `6.2.3`
6. Swashbuckle.Core: `5.6.0` ?? (Deprecated)

### External Dependencies
- **Database**: SQL Server (connection string in appsettings.json)
- **Server**: PT62\SQL2022 (SQL Server 2022 - fully compatible with .NET 10)
- **Database Name**: Gestion_Etudiants

## 4. Project-by-Project Plans

### Project: API Gestionnaire de Vacataire

**Project Type**: ASP.NET Core Web API  
**Project Path**: `API Gestionnaire de Vacataire.csproj`

#### Current State
- **Target Framework**: `net9.0`
- **Package Count**: 6 NuGet packages
- **Database Provider**: Entity Framework Core 3.1.22 with SQL Server
- **API Documentation**: Swagger/Swashbuckle
- **Serialization**: Newtonsoft.Json
- **Project Structure**: Controllers, Models, Data access layer

#### Target State
- **Target Framework**: `net10.0`
- **Package Count**: 5 NuGet packages (removing deprecated Swashbuckle.Core)
- **Updated Packages**: All packages upgraded to .NET 10 compatible versions

#### Migration Steps

##### 1. Prerequisites
**Verify .NET 10 SDK Installation**:
- Confirm .NET 10 SDK installed on development machine
- Check via: `dotnet --list-sdks`
- If missing, download from: https://dotnet.microsoft.com/download/dotnet/10.0

**Database Backup** (Recommended):
- Backup `Gestion_Etudiants` database before migration
- SQL Server 2022 fully supports .NET 10, but backup provides safety net

##### 2. Project File Update

**Update `API Gestionnaire de Vacataire.csproj`**:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <RootNamespace>API_Gestionnaire_de_Vacataire</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <Folder Include="Data\" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.0">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="7.2.0" />
    <!-- Remove Swashbuckle.Core - deprecated and unnecessary -->
  </ItemGroup>

</Project>
```

**Changes Summary**:
- `TargetFramework`: `net9.0` ? `net10.0`
- `Microsoft.EntityFrameworkCore`: `3.1.22` ? `10.0.0`
- `Microsoft.EntityFrameworkCore.SqlServer`: `3.1.22` ? `10.0.0`
- `Microsoft.EntityFrameworkCore.Tools`: `3.1.22` ? `10.0.0`
- `Newtonsoft.Json`: `13.0.1` ? `13.0.3` (latest stable)
- `Swashbuckle.AspNetCore`: `6.2.3` ? `7.2.0`
- **Remove**: `Swashbuckle.Core` (deprecated, functionality covered by Swashbuckle.AspNetCore)

##### 3. Package Update Reference

| Package | Current Version | Target Version | Update Reason | Breaking Changes Expected |
|---------|----------------|----------------|---------------|--------------------------|
| Microsoft.EntityFrameworkCore | 3.1.22 | 10.0.0 | .NET 10 compatibility, 7 major version jump | **High** - See EF Core breaking changes catalog |
| Microsoft.EntityFrameworkCore.SqlServer | 3.1.22 | 10.0.0 | Must match EF Core version | **High** - Provider changes, query translation |
| Microsoft.EntityFrameworkCore.Tools | 3.1.22 | 10.0.0 | Must match EF Core version | **Medium** - Migration tooling updates |
| Newtonsoft.Json | 13.0.1 | 13.0.3 | Bug fixes, latest stable | **None** - Patch version update |
| Swashbuckle.AspNetCore | 6.2.3 | 7.2.0 | .NET 10 compatibility | **Low** - Minor API adjustments |
| Swashbuckle.Core | 5.6.0 | **REMOVE** | Deprecated, redundant | N/A - Remove entirely |

##### 4. Expected Breaking Changes

**Entity Framework Core 3.1 ? 10.0 Breaking Changes**:

**A. DbContext Configuration Changes**

**EF Core 3.1 Pattern**:
```csharp
// Startup.cs
services.AddDbContext<DbGestionnaireStagiaireContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DBGV")));
```

**EF Core 10.0 Recommended Pattern** (if using Program.cs top-level statements):
```csharp
// Program.cs
builder.Services.AddDbContext<DbGestionnaireStagiaireContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBGV")));
```

**Note**: If project still uses Startup.cs pattern (older style), it will continue to work but may show warnings. Configuration syntax remains similar.

**B. Query Translation Changes**

**Breaking Change**: EF Core 6+ improved query translation but some complex LINQ queries may fail to translate or behave differently.

**Common Issues**:
1. **Client Evaluation**: EF Core 3.1 allowed some client evaluation; EF Core 5+ requires explicit `.AsEnumerable()`
2. **String Functions**: `string.IsNullOrEmpty()` now translates differently
3. **Navigation Properties**: Eager loading syntax may require updates

**Example Fix**:
```csharp
// May fail in EF Core 10 if complex
var results = dbContext.Students
    .Where(s => SomeComplexMethod(s.Name))
    .ToList();

// Fix: Evaluate in memory
var results = dbContext.Students
    .AsEnumerable()  // Switch to client evaluation
    .Where(s => SomeComplexMethod(s.Name))
    .ToList();
```

**C. Migration File Compatibility**

**Issue**: EF Core 3.1 migration files may use deprecated APIs in EF Core 10.0.

**Resolution**:
1. Existing migrations should work but may generate warnings
2. If errors occur, regenerate migrations:
   ```bash
   dotnet ef migrations add InitialCreate --force
   ```
3. Verify database schema matches after regeneration

**D. Tracking Behavior**

**Change**: EF Core 5+ changed default tracking behavior for some queries.

**Recommendation**: Explicitly specify tracking behavior:
```csharp
// For read-only queries
var students = dbContext.Students.AsNoTracking().ToList();

// For queries requiring updates
var students = dbContext.Students.AsTracking().ToList();
```

**E. Connection Resiliency**

**Enhancement**: EF Core 6+ improved connection resiliency. May want to enable:
```csharp
options.UseSqlServer(
    connectionString,
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
```

**F. Swashbuckle Changes**

**Swashbuckle.Core Removal**: 
- `Swashbuckle.Core` is deprecated and incompatible with .NET 6+
- `Swashbuckle.AspNetCore` provides all necessary functionality
- Swagger configuration in `Startup.cs` or `Program.cs` should use `AddSwaggerGen()` and `UseSwagger()` from `Swashbuckle.AspNetCore`

**Verify Swagger Configuration**:
```csharp
// Startup.cs or Program.cs
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gestionnaire de Vacataire", Version = "v1" });
});

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gestionnaire de Vacataire v1"));
```

##### 5. Code Modifications

**Files Requiring Review**:

1. **Models/DbGestionnaireStagiaireContext.cs**
   - Review `OnConfiguring` method (if present) - connection string handling unchanged
   - Review `OnModelCreating` method - fluent API syntax mostly unchanged
   - Check for any EF Core 3.1 specific APIs marked obsolete

2. **Startup.cs** (or **Program.cs**)
   - Verify DbContext registration uses correct syntax
   - Ensure Swagger configuration uses `Swashbuckle.AspNetCore` only
   - Remove any references to `Swashbuckle.Core`

3. **Controllers**
   - Review LINQ queries for client evaluation issues
   - Add `.AsNoTracking()` for read-only queries (performance optimization)
   - Verify async/await patterns (no breaking changes expected)

4. **Configuration Files**
   - `appsettings.json`: No changes required (connection string format unchanged)
   - `appsettings.Development.json`: No changes required

**Specific Areas to Check**:

**DbContext File** (`Models/DbGestionnaireStagiaireContext.cs`):
```csharp
// Review this method if present
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    // If hardcoded connection string exists, verify format
    // Prefer dependency injection via Startup.cs/Program.cs
}

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Review entity configurations
    // Check for deprecated fluent API methods
    // EF Core 10 syntax should be compatible, but verify
}
```

**Controllers** (e.g., `Controllers/EmploiDeTempsControllers.cs`):
```csharp
// Example query to review
public async Task<IActionResult> GetAll()
{
    var emplois = await _context.EmploiDeTemps
        .AsNoTracking()  // Add for read-only queries
        .ToListAsync();
    return Ok(emplois);
}

// Complex queries may need refactoring
public async Task<IActionResult> GetFiltered(string filter)
{
    // If query fails, consider client evaluation
    var results = await _context.EmploiDeTemps
        .Where(e => /* complex expression */)
        .ToListAsync();
    
    // Or refactor to simpler LINQ
}
```

##### 6. Build and Restore

**Execute**:
```bash
cd "C:\Users\vngounou\source\repos\API-Gestionnaire-de-Vacataire"
dotnet restore
dotnet build
```

**Expected Initial Errors**:
- Obsolete API warnings from EF Core
- Potential LINQ query translation errors
- Missing namespace references (if Swashbuckle.Core was used directly)

**Resolution**:
- Address errors based on compiler messages
- Reference EF Core 10.0 documentation for obsolete API replacements: https://learn.microsoft.com/ef/core/what-is-new/ef-core-10.0/breaking-changes
- Update LINQ queries as needed

##### 7. Testing Strategy

**Unit Tests** (if present):
- Run all existing unit tests
- Verify database mocking still works
- Update test frameworks if needed (e.g., xUnit, NUnit should be compatible)

**Integration Tests**:

**A. Database Connectivity Test**:
```bash
# Verify connection string works
dotnet ef dbcontext info
```

**Expected Output**: Connection successful, database name displayed

**B. API Endpoint Tests**:
1. Start application: `dotnet run`
2. Navigate to Swagger UI: `https://localhost:{port}/swagger`
3. Test each endpoint:
   - GET endpoints (read operations)
   - POST endpoints (create operations)
   - PUT endpoints (update operations)
   - DELETE endpoints (delete operations)

**C. Database Query Validation**:
- Execute complex queries that use LINQ
- Verify results match expected data
- Check for SQL translation errors in logs
- Monitor performance (EF Core 10 may generate different SQL)

**D. Swagger UI Validation**:
- Verify Swagger page loads correctly
- Check all endpoints are documented
- Test "Try it out" functionality
- Ensure schemas are correctly displayed

##### 8. Validation Checklist

- [ ] Project builds without errors (`dotnet build` succeeds)
- [ ] Project builds without warnings (address EF Core obsolete APIs)
- [ ] Database context can be resolved from DI container
- [ ] Database connection successful (test with `dotnet ef dbcontext info`)
- [ ] All API endpoints respond correctly
- [ ] Swagger UI renders and shows all endpoints
- [ ] No runtime exceptions in EF Core query translation
- [ ] Database queries return expected results
- [ ] JSON serialization/deserialization works correctly
- [ ] Application configuration loads properly
- [ ] Logging functions as expected

**Performance Validation**:
- [ ] API response times comparable to pre-upgrade baseline
- [ ] Database query performance acceptable
- [ ] No memory leaks or excessive allocations

**Security Validation**:
- [ ] SQL Server authentication works (Trusted_Connection=True)
- [ ] Certificate validation configured (TrustServerCertificate=True)
- [ ] No new security warnings from package analyzer

#### Risk Level: High

**Primary Risk**: Entity Framework Core major version migration spanning 7 versions.

**Mitigation**:
- Comprehensive breaking changes catalog provided
- Incremental testing approach (build ? database ? API ? full integration)
- Database backup before migration
- Clear rollback strategy (git revert entire upgrade branch)

#### Dependencies
**External**:
- SQL Server 2022 database (`Gestion_Etudiants`)
- .NET 10 SDK installed on development machine

**Internal**: None (single project)

#### Estimated Complexity
**High** - Primarily due to Entity Framework Core migration complexity. Framework and other package updates are straightforward.

## 5. Risk Management

### High-Risk Changes

| Project | Risk Level | Description | Mitigation |
|---------|-----------|-------------|------------|
| API Gestionnaire de Vacataire | **High** | Entity Framework Core 3.1 ? 10.0 (7 major versions) | Comprehensive breaking changes catalog, test all database operations, verify migrations |
| API Gestionnaire de Vacataire | **Medium** | Swashbuckle.Core deprecation | Remove package, verify Swashbuckle.AspNetCore handles all Swagger needs |
| API Gestionnaire de Vacataire | **Low** | .NET 9 ? .NET 10 framework changes | Minimal breaking changes between these versions |

### Security Vulnerabilities
**Current Assessment**: No known critical security vulnerabilities detected in current package versions. However, upgrading from EF Core 3.1.22 (end-of-life since December 2022) to 10.0.x addresses multiple security patches released over the past 3 years.

**Recommended Action**: Proceed with all package updates as part of atomic upgrade.

### Entity Framework Core Breaking Changes (3.1 ? 10.0)

**Major Version Transitions Covered**: EF Core 3.1 ? 5.0 ? 6.0 ? 7.0 ? 8.0 ? 9.0 ? 10.0

#### Critical Breaking Changes

**1. Client Evaluation of Queries (EF Core 5.0)**
- **Impact**: Queries that relied on client evaluation will throw exceptions
- **Fix**: Use `.AsEnumerable()` to explicitly switch to client evaluation or rewrite LINQ queries
- **Example**: Complex string operations, custom methods in LINQ predicates

**2. Query Translation Improvements (EF Core 5.0+)**
- **Impact**: More queries translate to SQL, but some previously working queries may fail
- **Fix**: Review compiler errors, refactor unsupported LINQ patterns
- **Benefit**: Better performance for queries that now translate

**3. Navigation Property Changes (EF Core 5.0)**
- **Impact**: Required navigations now throw if null during query
- **Fix**: Use `.Include()` explicitly or configure navigations as optional

**4. SQL Server Provider Changes (EF Core 6.0+)**
- **Impact**: Improved query generation may produce different SQL
- **Fix**: Review query performance, update indexes if needed
- **Benefit**: Generally better SQL generation

**5. Migration Handling (EF Core 6.0+)**
- **Impact**: Migration file format changes, some obsolete APIs
- **Fix**: Existing migrations should work; regenerate only if errors occur
- **Tool**: Use `dotnet ef migrations add` with EF Core 10 tools

**6. Connection Resiliency (EF Core 6.0+)**
- **Impact**: New default behaviors for connection handling
- **Fix**: Configure explicitly if needed (shown in project plan)
- **Benefit**: Better handling of transient database errors

**7. Temporal Tables Support (EF Core 6.0+)**
- **Impact**: None if not using temporal tables
- **Benefit**: New feature available if needed

**8. JSON Columns (EF Core 7.0+)**
- **Impact**: None if not using JSON columns
- **Benefit**: New feature for storing JSON in SQL Server

**9. Bulk Update/Delete (EF Core 7.0+)**
- **Impact**: New `ExecuteUpdate()` and `ExecuteDelete()` methods available
- **Benefit**: More efficient bulk operations possible

**10. Primitive Collections (EF Core 8.0+)**
- **Impact**: Can now map arrays/lists of primitives directly
- **Benefit**: Simplifies some data models

**11. Complex Types (EF Core 8.0+)**
- **Impact**: Owned types behavior refined
- **Fix**: Review owned entity configurations if used

**12. AOT and Trimming (EF Core 8.0+)**
- **Impact**: Better support for AOT compilation
- **Benefit**: Potential deployment size reduction (opt-in)

**13. Performance Improvements (EF Core 9.0+)**
- **Impact**: Query compilation caching improvements
- **Benefit**: Better runtime performance

**14. LINQ Enhancements (EF Core 10.0)**
- **Impact**: More complex LINQ queries now supported
- **Benefit**: Less need for raw SQL

### Comprehensive Risk Mitigation Strategy

**Phase 1: Pre-Migration**
1. ? **Database Backup**: Create backup of `Gestion_Etudiants` database
2. ? **Branch Isolation**: Use `upgrade-to-NET10` branch (already created)
3. ? **Baseline Metrics**: Document current API response times

**Phase 2: During Migration**
1. **Incremental Testing**: Build ? Database ? API sequence
2. **Error Logging**: Enable verbose EF Core logging to catch query issues
3. **Query Review**: Test all database operations thoroughly

**Phase 3: Post-Migration**
1. **Performance Comparison**: Compare API response times to baseline
2. **Integration Testing**: Full end-to-end API workflow validation
3. **Database Integrity**: Verify data consistency

### Contingency Plans

#### Blocking Issue: EF Core Migration Compatibility
**If**: Existing database migrations incompatible with EF Core 10.0  
**Then**: 
1. Generate fresh snapshot with EF Core 10.0
2. Compare with existing schema using `dotnet ef database update --script`
3. Create manual migration if needed

**Alternative**: Use EF Core reverse engineering to regenerate context from existing database:
```bash
dotnet ef dbcontext scaffold "Server=PT62\SQL2022;Database=Gestion_Etudiants;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c DbGestionnaireStagiaireContext --force
```

#### Blocking Issue: Query Translation Failures
**If**: Complex LINQ queries fail to translate in EF Core 10.0  
**Then**:
1. Identify failed queries from compilation/runtime errors
2. Refactor using EF Core 10.0 compatible patterns (see documentation)
3. Use `.AsEnumerable()` for queries that must evaluate client-side
4. Consider raw SQL for highly complex queries: `context.Database.SqlQuery<T>()`

**Alternative**: Temporarily downgrade to EF Core 9.0 and upgrade incrementally (EF Core 8 ? 9 ? 10)

#### Blocking Issue: Swagger UI Breaks
**If**: Swagger UI doesn't render after removing Swashbuckle.Core  
**Then**:
1. Verify `Swashbuckle.AspNetCore` 7.2.0 installed
2. Check `Startup.cs`/`Program.cs` configuration
3. Ensure `UseSwagger()` and `UseSwaggerUI()` called in correct order
4. Review for any custom Swagger filters or configuration

**Alternative**: Use Swashbuckle.AspNetCore samples: https://github.com/domaindrivendev/Swashbuckle.AspNetCore

#### Performance Regression
**If**: API response times degrade after upgrade  
**Then**:
1. Enable EF Core logging: `optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)`
2. Review generated SQL queries (EF Core 10 may generate different SQL)
3. Add explicit `.AsNoTracking()` for read-only queries
4. Consider adding database indexes if new query patterns emerge

**Fallback**: Roll back upgrade and analyze specific performance issues:
```bash
git checkout master
git branch -D upgrade-to-NET10
```

#### Compilation Errors
**If**: Build fails with unresolved errors  
**Then**:
1. Review error messages carefully - note file and line number
2. Consult EF Core breaking changes documentation: https://learn.microsoft.com/ef/core/what-is-new/ef-core-10.0/breaking-changes
3. Search for error messages in EF Core GitHub issues
4. Refactor code using updated APIs

**Escalation Path**: Seek help in EF Core community forums or GitHub discussions

### Rollback Strategy

**Single Atomic Commit Approach**:
```bash
# If migration completed but issues discovered
git revert <commit-hash>

# Or delete branch entirely and start over
git checkout master
git branch -D upgrade-to-NET10
# Restore database from backup if migrations applied
```

**Database Rollback**:
```sql
-- Restore from backup
USE master;
RESTORE DATABASE Gestion_Etudiants 
FROM DISK = 'C:\Backup\Gestion_Etudiants_PreUpgrade.bak'
WITH REPLACE;
```

### Risk Acceptance

**Known Risks Being Accepted**:
1. **Large Version Jump**: Upgrading 7 EF Core major versions simultaneously (alternatives would extend timeline significantly)
2. **Limited Testing Window**: Single project allows focused testing but real-world edge cases may emerge post-deployment
3. **Query Behavior Changes**: EF Core 10 may generate different SQL; monitoring required post-upgrade

**Risk Owner**: Development team responsible for validating migration success

## 6. Testing & Validation Strategy

### Multi-Level Testing Approach

The testing strategy follows a progressive validation model, ensuring each layer works before moving to the next.

---

### Level 1: Build Verification

**Objective**: Ensure project compiles without errors or warnings

**Steps**:
1. Clean solution: `dotnet clean`
2. Restore packages: `dotnet restore`
3. Build project: `dotnet build --configuration Release`

**Success Criteria**:
- ? No compilation errors
- ? No warnings related to obsolete APIs
- ? All packages restored successfully
- ? Build output shows `net10.0` target framework

**Failure Response**: 
- Review compiler errors
- Consult EF Core breaking changes documentation
- Update code to address obsolete API usage

---

### Level 2: Database Connectivity

**Objective**: Verify Entity Framework Core can connect to database

**Steps**:
1. Verify DbContext registration:
   ```bash
   dotnet ef dbcontext info --project "API Gestionnaire de Vacataire.csproj"
   ```

2. Check connection string resolution:
   - Confirm `appsettings.json` connection string loads correctly
   - Verify `Server=PT62\SQL2022;Database=Gestion_Etudiants` accessible

3. Test migrations status:
   ```bash
   dotnet ef migrations list --project "API Gestionnaire de Vacataire.csproj"
   ```

**Success Criteria**:
- ? `dotnet ef dbcontext info` displays database and connection details
- ? No connection errors
- ? Migrations list displays (if migrations exist)
- ? DbContext type resolved correctly

**Failure Response**:
- Check SQL Server is running
- Verify SQL Server 2022 instance name (PT62\SQL2022)
- Confirm database `Gestion_Etudiants` exists
- Review DbContext configuration in Startup.cs/Program.cs

---

### Level 3: Database Query Execution

**Objective**: Validate LINQ queries translate and execute correctly

**Test Cases**:

**A. Simple Query Test**:
```csharp
// In controller or test class
var firstRecord = await _context.YourEntity.FirstOrDefaultAsync();
// Verify returns data or null without exception
```

**B. Complex Query Test**:
```csharp
// Test filtering and sorting
var filtered = await _context.YourEntity
    .Where(e => e.SomeProperty == value)
    .OrderBy(e => e.AnotherProperty)
    .ToListAsync();
```

**C. Navigation Property Test**:
```csharp
// Test eager loading
var withRelations = await _context.YourEntity
    .Include(e => e.RelatedEntity)
    .ToListAsync();
```

**D. Write Operation Test**:
```csharp
// Test insert
var newEntity = new YourEntity { /* properties */ };
_context.YourEntity.Add(newEntity);
await _context.SaveChangesAsync();

// Test update
newEntity.SomeProperty = "updated";
await _context.SaveChangesAsync();

// Test delete
_context.YourEntity.Remove(newEntity);
await _context.SaveChangesAsync();
```

**Success Criteria**:
- ? All queries execute without exceptions
- ? Data returned matches expected results
- ? No LINQ translation errors in logs
- ? Write operations commit successfully

**Failure Response**:
- Enable EF Core logging to see generated SQL
- Review LINQ query patterns
- Consult EF Core 10.0 query documentation
- Refactor queries using `.AsEnumerable()` if needed

---

### Level 4: API Endpoint Testing

**Objective**: Verify all API endpoints function correctly

**Method**: Use Swagger UI for comprehensive endpoint testing

**Steps**:
1. Start application: 
   ```bash
   dotnet run --project "API Gestionnaire de Vacataire.csproj"
   ```

2. Navigate to Swagger UI: `https://localhost:{port}/swagger`

3. Test each controller systematically:

**For Each Endpoint**:
- **GET Endpoints**: 
  - Test without parameters (get all)
  - Test with ID parameter (get single)
  - Test with query parameters (filtering)
  - Verify JSON response structure
  
- **POST Endpoints**: 
  - Test with valid payload
  - Test with invalid payload (validation)
  - Verify 201 Created response
  - Confirm entity created in database

- **PUT Endpoints**: 
  - Test with valid ID and payload
  - Test with non-existent ID (404)
  - Verify entity updated in database

- **DELETE Endpoints**: 
  - Test with valid ID
  - Test with non-existent ID (404)
  - Confirm entity removed from database

**Success Criteria**:
- ? All endpoints respond with correct HTTP status codes
- ? Response payloads match expected schema
- ? Database operations reflect in subsequent queries
- ? Error responses include appropriate messages
- ? No unhandled exceptions in application logs

**Specific Validations for EmploiDeTemps Controller**:
- GET /api/emploidetemps - Returns all records
- GET /api/emploidetemps/{id} - Returns single record
- POST /api/emploidetemps - Creates new record
- PUT /api/emploidetemps/{id} - Updates existing record
- DELETE /api/emploidetemps/{id} - Deletes record

**Failure Response**:
- Review controller action code
- Check model validation attributes
- Verify DbContext injection
- Review error logs for stack traces

---

### Level 5: Swagger Documentation Validation

**Objective**: Ensure Swagger UI renders correctly after Swashbuckle.Core removal

**Verification Points**:
1. **UI Loads**: Swagger page loads without JavaScript errors
2. **All Endpoints Listed**: Each controller action appears in documentation
3. **Schemas Defined**: Model schemas displayed correctly
4. **Try It Out Works**: Interactive testing functions properly
5. **Authorization** (if applicable): Auth UI renders correctly

**Success Criteria**:
- ? Swagger UI page loads at `/swagger`
- ? All API endpoints documented
- ? Request/response schemas visible
- ? "Try it out" functionality works
- ? No console errors in browser developer tools

**Failure Response**:
- Verify `Swashbuckle.AspNetCore` 7.2.0 installed
- Check `UseSwagger()` and `UseSwaggerUI()` middleware order
- Review `AddSwaggerGen()` configuration
- Ensure no remaining references to `Swashbuckle.Core`

---

### Level 6: Integration & Smoke Testing

**Objective**: Validate end-to-end workflows

**Test Scenarios**:

**Scenario 1: Full CRUD Workflow**
1. Create a new EmploiDeTemps via POST
2. Retrieve it via GET by ID
3. Update it via PUT
4. List all EmploiDeTemps via GET (confirm presence)
5. Delete it via DELETE
6. Confirm deletion via GET by ID (404)

**Scenario 2: Data Integrity**
1. Create records with foreign key relationships (if applicable)
2. Verify navigation properties load correctly
3. Test cascading deletes (if configured)

**Scenario 3: Error Handling**
1. Test invalid data submission
2. Test duplicate key scenarios
3. Test authorization failures (if auth implemented)

**Success Criteria**:
- ? Complete workflows execute without errors
- ? Database state consistent throughout operations
- ? Validation errors handled gracefully
- ? Expected exceptions return appropriate HTTP status codes

---

### Level 7: Performance & Load Testing

**Objective**: Ensure acceptable performance after upgrade

**Baseline Metrics** (capture before upgrade):
- Average API response time per endpoint
- Database query execution times
- Memory usage under load

**Post-Upgrade Validation**:
1. **Response Time Comparison**:
   - Measure same endpoints with same data
   - Accept ±20% variance as normal
   - Investigate outliers

2. **SQL Query Analysis**:
   - Enable EF Core SQL logging
   - Compare generated SQL to baseline (if available)
   - Check for N+1 query issues
   - Verify indexes still utilized

3. **Memory Profiling** (optional):
   - Monitor memory usage during typical operations
   - Check for memory leaks over extended run

**Success Criteria**:
- ? Response times within acceptable range
- ? No significant performance degradation
- ? Database queries efficient
- ? No memory leaks detected

**Note**: EF Core 10 often improves performance, but query changes may affect specific endpoints.

---

### Phase-by-Phase Testing Summary

#### Phase 1: Atomic Upgrade Testing

**After completing project file and package updates**:
1. ? Build Verification (Level 1)
2. ? Database Connectivity (Level 2)
3. ? Query Execution (Level 3)

**Checkpoint**: Project must build and connect to database before proceeding to API testing.

#### Phase 2: Validation Testing

**After build and database validation**:
4. ? API Endpoint Testing (Level 4)
5. ? Swagger Validation (Level 5)
6. ? Integration Testing (Level 6)
7. ? Performance Validation (Level 7)

**Checkpoint**: All tests must pass before considering upgrade complete.

---

### Testing Tools & Environment

**Required Tools**:
- .NET 10 SDK (for build and EF Core CLI)
- Web browser (for Swagger UI testing)
- SQL Server Management Studio or Azure Data Studio (for database verification)
- Postman or similar (optional, for additional API testing)

**Test Environment**:
- Local development machine
- SQL Server 2022 (PT62\SQL2022)
- Database: Gestion_Etudiants

**Optional Tools**:
- EF Core logging enabled for SQL inspection
- Performance profiling tools (dotTrace, PerfView)
- Browser developer tools for Swagger UI validation

---

### Comprehensive Validation Checklist

**Pre-Migration**:
- [ ] Database backup created
- [ ] Baseline performance metrics captured
- [ ] Upgrade branch created and checked out

**Build Phase**:
- [ ] `dotnet clean` successful
- [ ] `dotnet restore` successful
- [ ] `dotnet build` successful with 0 errors
- [ ] No obsolete API warnings

**Database Phase**:
- [ ] `dotnet ef dbcontext info` successful
- [ ] Connection string resolves correctly
- [ ] DbContext type registered in DI
- [ ] Database accessible

**Query Phase**:
- [ ] Simple queries execute
- [ ] Complex queries execute
- [ ] Navigation properties load
- [ ] Write operations succeed

**API Phase**:
- [ ] Application starts without errors
- [ ] All GET endpoints respond correctly
- [ ] All POST endpoints create records
- [ ] All PUT endpoints update records
- [ ] All DELETE endpoints remove records
- [ ] Error handling works correctly

**Swagger Phase**:
- [ ] Swagger UI page loads
- [ ] All endpoints documented
- [ ] Schemas display correctly
- [ ] "Try it out" works

**Integration Phase**:
- [ ] Full CRUD workflow completes
- [ ] Data integrity maintained
- [ ] Error scenarios handled

**Performance Phase**:
- [ ] Response times acceptable
- [ ] SQL queries efficient
- [ ] No performance regressions

**Final Validation**:
- [ ] All tests passed
- [ ] No unresolved errors or warnings
- [ ] Documentation updated
- [ ] Changes committed to upgrade branch

## 7. Complexity & Effort Assessment

### Per-Project Complexity

| Project | Complexity | Dependencies | Packages | Risk | Key Challenges |
|---------|-----------|--------------|----------|------|----------------|
| API Gestionnaire de Vacataire | **High** | 0 projects, 6 packages | 6 ? 5 | High | EF Core 3.1 ? 10.0 migration, 7 major version jump |

### Phase Complexity Assessment

**Phase 1: Atomic Upgrade**
- **Complexity**: High
- **Primary Challenge**: Entity Framework Core major version migration
- **Secondary Challenges**: 
  - Deprecated package removal (Swashbuckle.Core)
  - Breaking changes across EF Core 4.x, 5.x, 6.x, 7.x, 8.x, 9.x, 10.x
  - DbContext configuration updates
  - Potential LINQ query translation changes
- **Expected Effort**: High (due to EF Core breaking changes)

**Phase 2: Validation**
- **Complexity**: Medium
- **Primary Challenge**: Comprehensive database operation testing
- **Secondary Challenges**:
  - API endpoint validation
  - Swagger UI verification
  - Database query correctness
- **Expected Effort**: Medium (thorough testing required)

### Relative Complexity Ratings

**Low Complexity**:
- Target framework property update (`net9.0` ? `net10.0`)
- Newtonsoft.Json version update (minor version bump)
- Swashbuckle.AspNetCore version update

**Medium Complexity**:
- Swagger documentation verification after Swashbuckle.Core removal
- ASP.NET Core middleware compatibility checks
- Configuration validation

**High Complexity**:
- Entity Framework Core 3.1.22 ? 10.0.x migration
- DbContext and model configuration updates
- Database query pattern updates
- Migration file compatibility verification

### Resource Requirements

**Skill Levels Needed**:
- .NET/C# proficiency: Intermediate to Advanced
- Entity Framework Core expertise: **Advanced** (critical for EF Core migration)
- ASP.NET Core knowledge: Intermediate
- SQL Server familiarity: Basic to Intermediate
- Git version control: Basic

**Parallel Capacity**:
- **N/A** - Single project migrated sequentially

**Testing Requirements**:
- Database access for validation
- Ability to run API locally
- Familiarity with Swagger/API testing tools

### Dependency Ordering
**N/A** - Single project, no dependency ordering required

### Key Success Factors
1. **EF Core Expertise**: Understanding of EF Core breaking changes across 7 major versions
2. **Comprehensive Testing**: Database operations must be validated thoroughly
3. **Incremental Validation**: Build ? Test Database ? Test API endpoints sequentially
4. **Rollback Readiness**: Clear rollback plan if critical issues discovered

## 8. Source Control Strategy

### Branching Strategy

**Branch Structure**:
```
master (or main)
  ??? upgrade-to-NET10 ? All migration work happens here
```

**Current State**:
- ? **Source Branch**: `master`
- ? **Upgrade Branch**: `upgrade-to-NET10` (created and checked out)
- ? **Pending Changes**: Committed before branch creation

**Merge Strategy**: 
- Single pull request from `upgrade-to-NET10` to `master` after all validation complete
- Squash merge optional (consider single commit for atomic upgrade)

---

### Commit Strategy

**All-At-Once Single Commit Approach** (Recommended)

Since this is an atomic upgrade with all changes interdependent, use a single comprehensive commit:

**Commit Structure**:
```bash
git add .
git commit -m "Upgrade to .NET 10.0

- Update target framework from net9.0 to net10.0
- Upgrade Entity Framework Core from 3.1.22 to 10.0.0
- Upgrade Swashbuckle.AspNetCore from 6.2.3 to 7.2.0
- Remove deprecated Swashbuckle.Core package
- Update Newtonsoft.Json from 13.0.1 to 13.0.3
- Address EF Core 10.0 breaking changes in DbContext
- Update LINQ queries for EF Core 10 compatibility
- Verify all API endpoints functional
- Validate Swagger UI renders correctly

Tested:
- Build successful with 0 errors/warnings
- Database connectivity verified
- All API endpoints tested via Swagger
- CRUD operations validated
- Performance baseline acceptable

Breaking changes addressed:
- EF Core query translation updates
- Client evaluation patterns fixed
- DbContext configuration updated
- Migration compatibility verified"
```

**Rationale for Single Commit**:
- ? Project file, packages, and code changes are inseparable
- ? Easier to revert if issues discovered
- ? Clear atomic unit in Git history
- ? Simplifies code review (one comprehensive change)

**Alternative: Multi-Commit Approach**

If prefer to track progress incrementally:

**Commit 1: Project File & Package Updates**
```bash
git commit -m "Update project file for .NET 10

- Set TargetFramework to net10.0
- Update EF Core packages to 10.0.0
- Update Swashbuckle.AspNetCore to 7.2.0
- Remove Swashbuckle.Core
- Update Newtonsoft.Json to 13.0.3"
```

**Commit 2: Code Updates**
```bash
git commit -m "Address EF Core 10 breaking changes

- Update DbContext configuration
- Fix LINQ query patterns for EF Core 10
- Add AsNoTracking() for read queries
- Update migration handling"
```

**Commit 3: Validation Complete**
```bash
git commit -m "Complete .NET 10 upgrade validation

- All tests passed
- API endpoints verified
- Swagger UI validated
- Performance acceptable"
```

**Choose based on team preference**. Single commit recommended for this atomic upgrade.

---

### Commit Message Guidelines

**Format**:
```
<Type>: <Short summary>

<Detailed description>

<Testing notes>

<Breaking changes if any>
```

**Types**:
- `upgrade:` Framework or major package version upgrade
- `fix:` Bug fixes found during migration
- `refactor:` Code changes for compatibility
- `docs:` Documentation updates

**Example**:
```
upgrade: Migrate to .NET 10 and EF Core 10

Updated all packages and framework references for .NET 10 compatibility.
Major change is Entity Framework Core upgrade from 3.1.22 to 10.0.0.

Testing:
- Build: Successful
- Database: Connected and queries functional
- API: All endpoints tested via Swagger
- Performance: Response times within baseline

Breaking Changes:
- EF Core LINQ query patterns updated for EF 10 translation
- Removed deprecated Swashbuckle.Core package
```

---

### Review and Merge Process

#### Pre-Review Checklist

Before creating pull request:
- [ ] All validation checklist items complete (§6)
- [ ] Build succeeds with 0 errors/warnings
- [ ] All API endpoints tested
- [ ] Swagger UI verified
- [ ] Database operations validated
- [ ] Performance acceptable
- [ ] Commit messages clear and descriptive

#### Pull Request Creation

**PR Title**: `Upgrade to .NET 10 and EF Core 10`

**PR Description Template**:
```markdown
## Overview
Upgrades API Gestionnaire de Vacataire from .NET 9 to .NET 10.

## Changes
- **Framework**: net9.0 ? net10.0
- **Entity Framework Core**: 3.1.22 ? 10.0.0
- **Swashbuckle.AspNetCore**: 6.2.3 ? 7.2.0
- **Removed**: Swashbuckle.Core (deprecated)
- **Updated**: Newtonsoft.Json 13.0.1 ? 13.0.3

## Breaking Changes Addressed
- EF Core query translation patterns updated
- DbContext configuration reviewed and validated
- LINQ queries tested for EF Core 10 compatibility

## Testing Performed
- ? Build successful (0 errors, 0 warnings)
- ? Database connectivity verified
- ? All API endpoints tested
- ? Swagger UI functional
- ? CRUD operations validated
- ? Performance baseline acceptable

## Migration Risks
- **High**: EF Core 3.1 ? 10.0 spans 7 major versions
- **Mitigation**: Comprehensive testing performed, database backup available

## Deployment Notes
- Requires .NET 10 SDK on target environment
- No database migration changes required
- No configuration file changes needed
- SQL Server 2022 fully compatible

## Rollback Plan
- Revert commit or merge
- Restore database backup if migrations applied
- Redeploy previous version
```

#### Code Review Checklist

**For Reviewers**:
- [ ] Project file changes correct (target framework, package versions)
- [ ] No deprecated packages remaining
- [ ] DbContext configuration appropriate
- [ ] LINQ queries follow EF Core 10 best practices
- [ ] Error handling maintained
- [ ] API contracts unchanged (no breaking API changes)
- [ ] Swagger configuration correct
- [ ] Testing evidence provided
- [ ] Commit messages clear

#### Merge Criteria

**Merge only when**:
- ? All validation tests passed
- ? Code review approved
- ? Build succeeds in CI/CD (if applicable)
- ? No blocking comments unresolved
- ? Documentation updated

**Merge Method**:
- **Squash Merge** (if multiple commits): Creates single commit in master
- **Merge Commit** (if single commit): Preserves branch history

---

### Post-Merge Actions

**After merge to master**:

1. **Tag Release** (optional):
   ```bash
   git tag -a v2.0.0-net10 -m "Upgraded to .NET 10"
   git push origin v2.0.0-net10
   ```

2. **Deploy to Test Environment**:
   - Verify .NET 10 SDK installed on server
   - Deploy application
   - Run smoke tests
   - Monitor logs for errors

3. **Monitor Production** (after production deployment):
   - Watch API response times
   - Monitor error logs
   - Check database query performance
   - Validate no EF Core exceptions

4. **Cleanup**:
   ```bash
   # Delete local upgrade branch after successful merge
   git branch -d upgrade-to-NET10
   
   # Delete remote upgrade branch (if pushed)
   git push origin --delete upgrade-to-NET10
   ```

5. **Document Upgrade**:
   - Update project README with new .NET version
   - Note any configuration changes for deployment
   - Document any new features available in .NET 10

---

### Continuous Integration Considerations

**If CI/CD pipeline exists**:

**Pipeline Updates Required**:
1. Update build agent to support .NET 10 SDK
2. Update Docker base images (if containerized): `mcr.microsoft.com/dotnet/aspnet:10.0`
3. Update deployment scripts to reference net10.0
4. Verify test runners compatible with .NET 10

**CI/CD Validation**:
- Build step must succeed
- Unit tests must pass (if present)
- Integration tests must pass
- Code coverage should not decrease significantly

**Deployment Checklist**:
- [ ] .NET 10 runtime available on target servers
- [ ] Connection strings configured correctly
- [ ] Environment variables set
- [ ] Database accessible from application server
- [ ] Logging configured
- [ ] Health check endpoints responding

---

### Rollback Procedures

#### Scenario 1: Issues Found Before Merge

**Action**: Abandon upgrade branch
```bash
git checkout master
git branch -D upgrade-to-NET10
# Restore database from backup if needed
```

#### Scenario 2: Issues Found After Merge (Before Production)

**Action**: Revert merge commit
```bash
git revert -m 1 <merge-commit-hash>
git push origin master
# Redeploy previous version to test environment
```

#### Scenario 3: Issues Found in Production

**Action**: Emergency rollback
```bash
# Revert the upgrade commit
git revert <commit-hash>
git push origin master

# Restore database backup
# USE master;
# RESTORE DATABASE Gestion_Etudiants 
# FROM DISK = 'C:\Backup\Gestion_Etudiants_PreUpgrade.bak'
# WITH REPLACE;

# Redeploy previous version immediately
```

**Incident Response**:
1. Notify team of rollback
2. Document issues encountered
3. Plan remediation strategy
4. Re-attempt upgrade after fixes

---

### Branch Protection (Recommended)

**If using GitHub/Azure DevOps/GitLab**:

**Configure master branch protection**:
- ? Require pull request reviews (at least 1 approver)
- ? Require status checks to pass (CI build)
- ? Require branches to be up to date
- ? Restrict force push
- ? Restrict deletions

**Benefits**:
- Prevents accidental direct commits to master
- Ensures code review process followed
- Maintains clean Git history

---

### Communication Plan

**Team Notifications**:

**Before Starting**:
- Notify team upgrade is beginning
- Share plan.md document
- Communicate expected timeline

**During Upgrade**:
- Share progress updates
- Report blockers immediately
- Document solutions to issues

**After Completion**:
- Announce upgrade complete
- Share testing results
- Provide deployment instructions

**Channels**:
- Team chat (Slack/Teams)
- Email for formal notifications
- Project management tool (Jira/Azure DevOps)

---

### Git Best Practices for This Upgrade

1. **Commit Often** (if using multi-commit approach): Save progress at logical checkpoints
2. **Write Clear Messages**: Explain what changed and why
3. **Test Before Commit**: Never commit broken code
4. **Keep Branch Clean**: Don't merge unrelated changes
5. **Sync Regularly**: Keep upgrade branch updated if master changes (unlikely during short upgrade)
6. **Document Issues**: Note any problems encountered and how resolved

### Version Control Artifacts

**Files Generated During Upgrade**:
- `API Gestionnaire de Vacataire.csproj` (modified)
- Source code files (if breaking changes fixed)
- `plan.md` (this document - reference artifact)
- `assessment.md` (reference artifact)

**Files NOT to Commit**:
- `bin/` directory
- `obj/` directory
- `.vs/` directory
- User-specific settings
- Database backup files
- Temporary test files

**Verify `.gitignore` includes**:
```
bin/
obj/
.vs/
*.user
*.suo
*.bak
```

## 9. Success Criteria

### Technical Criteria

The .NET 10 upgrade is considered technically successful when all of the following conditions are met:

#### Build & Compilation
- ? **Project Builds Successfully**: `dotnet build` completes with exit code 0
- ? **Zero Compilation Errors**: No build errors in any project file
- ? **Zero Warnings**: No obsolete API warnings or deprecation notices
- ? **Correct Target Framework**: Project file specifies `<TargetFramework>net10.0</TargetFramework>`
- ? **All Packages Restored**: `dotnet restore` completes successfully for all dependencies

#### Package Updates
- ? **Entity Framework Core Updated**: All EF Core packages at version 10.0.0 or higher
- ? **Swashbuckle Updated**: Swashbuckle.AspNetCore at version 7.2.0 or higher
- ? **Deprecated Packages Removed**: Swashbuckle.Core no longer referenced
- ? **Newtonsoft.Json Updated**: Version 13.0.3 or higher
- ? **No Package Conflicts**: `dotnet list package` shows no version conflicts
- ? **No Security Vulnerabilities**: All packages free of known vulnerabilities

#### Database Connectivity
- ? **DbContext Resolves**: `dotnet ef dbcontext info` executes successfully
- ? **Connection String Valid**: Application connects to `Gestion_Etudiants` database
- ? **SQL Server Compatible**: SQL Server 2022 (PT62\SQL2022) accessible
- ? **Migrations Status Known**: `dotnet ef migrations list` executes without errors

#### Database Operations
- ? **Simple Queries Execute**: Basic SELECT queries complete successfully
- ? **Complex Queries Execute**: Filtered, sorted, and joined queries work correctly
- ? **Navigation Properties Load**: Eager loading with `.Include()` functions properly
- ? **Write Operations Succeed**: INSERT, UPDATE, DELETE operations complete
- ? **Transactions Work**: SaveChanges() commits changes to database
- ? **No LINQ Translation Errors**: All LINQ queries translate to SQL or explicitly use client evaluation

#### API Functionality
- ? **Application Starts**: `dotnet run` starts application without errors
- ? **All GET Endpoints Respond**: Read operations return correct data
- ? **All POST Endpoints Work**: Create operations successfully add records
- ? **All PUT Endpoints Work**: Update operations modify existing records
- ? **All DELETE Endpoints Work**: Delete operations remove records
- ? **HTTP Status Codes Correct**: 200 OK, 201 Created, 404 Not Found, etc. as expected
- ? **JSON Serialization Works**: Request/response payloads serialize correctly
- ? **Error Handling Intact**: Invalid requests return appropriate error messages

#### Swagger Documentation
- ? **Swagger UI Loads**: `/swagger` endpoint renders page without errors
- ? **All Endpoints Documented**: Every controller action appears in Swagger
- ? **Schemas Display**: Request/response models show correct structure
- ? **Try It Out Functions**: Interactive testing works for all endpoints
- ? **No JavaScript Errors**: Browser console shows no Swagger-related errors

#### Configuration & Environment
- ? **appsettings.json Unchanged**: Configuration format remains compatible
- ? **Connection Strings Valid**: Database connection strings load correctly
- ? **Logging Works**: Application logs appear as expected
- ? **Environment Variables Resolved**: Any environment-specific settings load correctly

---

### Quality Criteria

Beyond basic functionality, the upgrade maintains code quality standards:

#### Code Quality Maintained
- ? **No Code Degradation**: Code structure and patterns remain clean
- ? **Consistent Style**: Coding conventions maintained throughout
- ? **Proper Error Handling**: Exception handling patterns preserved or improved
- ? **Null Reference Safety**: No new null reference warnings (if nullable reference types enabled)

#### Test Coverage Maintained
- ? **All Existing Tests Pass**: Unit tests continue to pass (if present)
- ? **Test Projects Build**: Test projects compile successfully (if present)
- ? **Test Frameworks Compatible**: xUnit/NUnit/MSTest compatible with .NET 10 (if present)
- ? **No Test Regressions**: Previously passing tests still pass

#### Performance Acceptable
- ? **Response Times Comparable**: API endpoints respond within baseline range (±20%)
- ? **Database Query Performance**: SQL queries execute efficiently
- ? **No Memory Leaks**: Application memory usage stable during operation
- ? **Startup Time Reasonable**: Application starts within acceptable timeframe

#### Documentation Updated
- ? **README Reflects .NET 10**: Project documentation mentions correct framework version
- ? **Deployment Notes Updated**: Any deployment-specific instructions revised
- ? **API Documentation Current**: Swagger descriptions accurate
- ? **Code Comments Accurate**: In-code documentation reflects any changes

---

### Process Criteria

The upgrade process followed established practices:

#### All-At-Once Strategy Followed
- ? **Atomic Upgrade Completed**: All project files, packages, and code updated together
- ? **Single Coordinated Operation**: No intermediate multi-targeting states
- ? **All Components Updated Simultaneously**: Framework and packages upgraded in one pass
- ? **No Partial Upgrades**: Complete upgrade of all dependencies

#### Dependency Order Respected
- ? **N/A for Single Project**: No dependency order concerns (single project solution)

#### Testing Strategy Executed
- ? **Build Verification Completed**: Level 1 testing passed
- ? **Database Connectivity Verified**: Level 2 testing passed
- ? **Query Execution Validated**: Level 3 testing passed
- ? **API Endpoints Tested**: Level 4 testing passed
- ? **Swagger Validated**: Level 5 testing passed
- ? **Integration Tests Passed**: Level 6 testing passed
- ? **Performance Validated**: Level 7 testing passed

#### Source Control Strategy Followed
- ? **Upgrade Branch Used**: All work performed on `upgrade-to-NET10` branch
- ? **Commits Clear**: Commit messages descriptive and meaningful
- ? **Changes Reviewed**: Code review completed before merge (if team process)
- ? **Single Atomic Commit** (or logical multi-commit): Changes organized appropriately
- ? **Master Branch Protected**: No direct commits to master

#### Risk Management Applied
- ? **Database Backed Up**: Backup created before upgrade (recommended)
- ? **Breaking Changes Addressed**: All EF Core breaking changes handled
- ? **Contingency Plans Available**: Rollback procedures documented and understood
- ? **Validation Comprehensive**: All testing levels completed

---

### Business Criteria

The upgrade delivers business value:

#### Functionality Preserved
- ? **No Feature Regressions**: All existing features work as before
- ? **API Contracts Unchanged**: Client applications unaffected
- ? **Data Integrity Maintained**: Database operations produce correct results
- ? **User Experience Preserved**: No negative impact on API consumers

#### Security Improved
- ? **Latest Framework**: .NET 10 LTS includes latest security patches
- ? **Updated Dependencies**: All packages current with security fixes
- ? **EF Core Vulnerabilities Addressed**: EF Core 3.1 (EOL) vulnerabilities mitigated
- ? **No New Vulnerabilities**: Package analyzer shows no new security issues

#### Maintainability Enhanced
- ? **Supported Framework**: .NET 10 LTS supported until November 2027
- ? **Modern Dependencies**: All packages actively maintained
- ? **Future-Proof**: Foundation for future .NET updates
- ? **Technical Debt Reduced**: Removed deprecated packages (Swashbuckle.Core)

#### Deployment Ready
- ? **.NET 10 SDK Available**: Target deployment environment has .NET 10 runtime
- ? **Deployment Instructions Clear**: Plan includes deployment notes
- ? **Rollback Plan Documented**: Procedures for rollback defined
- ? **Monitoring Strategy**: Plan for post-deployment monitoring exists

---

### Acceptance Checklist

**Final validation before considering upgrade complete**:

#### Pre-Deployment Validation
- [ ] All technical criteria met (build, packages, database, API, Swagger)
- [ ] All quality criteria met (code quality, tests, performance, documentation)
- [ ] All process criteria met (strategy, testing, source control, risk management)
- [ ] Code review completed and approved
- [ ] Pull request merged to master
- [ ] Release tagged (optional but recommended)

#### Deployment Validation
- [ ] Application deployed to test/staging environment
- [ ] Smoke tests passed in deployment environment
- [ ] Database connectivity verified in deployment environment
- [ ] API endpoints accessible from expected clients
- [ ] Logs show no critical errors
- [ ] Performance monitoring active

#### Production Validation (if applicable)
- [ ] Application deployed to production successfully
- [ ] Health check endpoints responding
- [ ] No error spikes in monitoring
- [ ] API response times acceptable
- [ ] Database performance normal
- [ ] No user-reported issues

#### Documentation & Communication
- [ ] Upgrade completion communicated to team
- [ ] Deployment notes shared with operations team
- [ ] README and documentation updated
- [ ] Lessons learned documented (if any issues encountered)
- [ ] Upgrade artifacts archived (plan.md, assessment.md)

---

### Definition of Done

**The .NET 10 upgrade is DONE when**:

1. ? **All Technical Criteria Met**: Project builds, runs, and functions correctly on .NET 10
2. ? **All Quality Criteria Met**: Code quality, tests, and performance maintained
3. ? **All Process Criteria Met**: Upgrade strategy followed, testing completed, source control practices applied
4. ? **All Business Criteria Met**: Functionality preserved, security improved, maintainability enhanced
5. ? **Deployed Successfully**: Application running in target environment without issues
6. ? **Validated in Production**: Real-world usage confirms upgrade success (if applicable)
7. ? **Team Acknowledges Completion**: Stakeholders agree upgrade objectives achieved

---

### Success Metrics

**Quantifiable measures of success**:

| Metric | Target | Measurement Method |
|--------|--------|-------------------|
| Build Success | 100% (0 errors) | `dotnet build` exit code |
| Package Compatibility | 100% (all updated) | `dotnet list package` output |
| API Endpoint Functionality | 100% (all working) | Swagger UI testing |
| Test Pass Rate | 100% (if tests exist) | `dotnet test` results |
| Response Time Variance | ±20% from baseline | Performance monitoring |
| Security Vulnerabilities | 0 critical/high | Package vulnerability scan |
| Deployment Success | 1st attempt | Deployment log analysis |

---

### Post-Upgrade Success Indicators

**Ongoing indicators that upgrade was successful**:

**Short-term (First Week)**:
- No critical bugs reported
- API response times stable
- Error rates normal
- Database performance acceptable

**Medium-term (First Month)**:
- No performance degradation
- No data integrity issues
- Development velocity maintained
- Team comfortable with .NET 10 features

**Long-term (3+ Months)**:
- Ability to adopt new .NET 10 features
- Easier package updates (all on current versions)
- Improved maintainability
- Foundation for future .NET 11/12 upgrades

---

### Failure Criteria (When to Rollback)

**Upgrade should be rolled back if**:

? **Critical Failures**:
- Application fails to build after multiple fix attempts
- Database connectivity permanently broken
- Data integrity compromised
- Critical API endpoints non-functional
- Unresolvable security vulnerabilities introduced

? **Severe Performance Issues**:
- Response times >50% slower than baseline
- Database queries timing out
- Memory leaks causing crashes
- Unacceptable startup time

? **Business Impact**:
- Client applications broken due to API changes
- Data loss or corruption
- Regulatory compliance violated
- Revenue-impacting features non-functional

**Rollback Decision Authority**: Development lead or technical manager

---

### Celebration Criteria ??

**The team should celebrate when**:
- ? All success criteria met
- ? Deployed to production successfully
- ? First week post-deployment without incidents
- ? Technical debt significantly reduced (EF Core modernized)
- ? Foundation laid for future .NET innovations

**Acknowledgments**:
- Planning effort invested
- Thorough testing completed
- Team collaboration during upgrade
- Successful execution of complex migration (EF Core 3.1 ? 10.0)

---

## Summary

This migration plan provides a comprehensive roadmap for upgrading API Gestionnaire de Vacataire from .NET 9 to .NET 10. The All-At-Once Strategy ensures an efficient, atomic upgrade that minimizes complexity while thoroughly addressing the most significant challenge: the Entity Framework Core 3.1 ? 10.0 migration spanning 7 major versions.

Success depends on careful execution of the testing strategy, attention to EF Core breaking changes, and validation at every level from build to API endpoints. With proper preparation, thorough testing, and adherence to this plan, the upgrade will modernize the application onto .NET 10 LTS, providing a stable, secure, and maintainable foundation for the next 3+ years of development.
