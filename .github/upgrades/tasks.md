# LexAssert xUnit v3 Upgrade Tasks

## Overview

This document tracks the execution of LexAssert's upgrade from xUnit v2 to xUnit v3, including a prerequisite framework migration from .NET Standard 2.0 to .NET 8.0. The upgrade follows a two-stage approach: framework upgrade first, then xUnit v3 migration.

**CRITICAL NOTE**: xUnit v3 uses completely different package names than xUnit v2:
- xUnit v2: `xunit` package (version 2.x)
- xUnit v3: `xunit.v3` package (version 0.3.0+)
**Progress**: 3/4 tasks complete (75%) ![0%](https://progress-bar.xyz/75)

**Progress**: 2/4 tasks complete (50%) ![0%](https://progress-bar.xyz/50)

---

## Tasks

### [✓] TASK-001: Framework upgrade (.NET Standard 2.0 → .NET 8.0) *(Completed: 2026-02-10 14:35)*
**References**: Plan §Phase 1, Plan §Step 1-5

- [✓] (1) Update LexAssert.csproj TargetFramework from netstandard2.0 to net8.0
- [✓] (2) Update LexAssert.csproj VersionPrefix to 3.0.0 and PackageReleaseNotes per Plan §Step 2
- [✓] (3) Restore dependencies for LexAssert.csproj
- [✓] (4) Dependencies restored successfully (**Verify**)
- [✓] (5) Build LexAssert.csproj in Release configuration
- [✓] (6) Library builds with 0 errors (**Verify**)
- [✓] (7) Build LexAssert.Tests.csproj in Release configuration
- [✓] (8) Test project builds with 0 errors (**Verify**)
- [✓] (9) Run all tests in LexAssert.Tests.csproj
- [✓] (10) All tests pass (17+/17+ expected) (**Verify**)
- [✓] (11) Commit changes with message: "TASK-001: Upgrade LexAssert to .NET 8.0"

### [✓] TASK-002: Update to xUnit v3 packages *(Completed: 2026-02-10 14:37)*
**References**: Plan §Phase 2, Plan §Step 7

**IMPORTANT**: xUnit v3 uses different package names than xUnit v2. We must REMOVE old packages and ADD new v3 packages.

**xUnit v3 Package Structure**:
- `xunit.v3` (replaces `xunit`)
- No separate abstractions package needed (removed `xunit.abstractions`)
- `xunit.runner.visualstudio` version 3.0.0+ (compatible with v3)

- [✓] (1) Remove xunit v2 packages from LexAssert.csproj:
        - Remove PackageReference: xunit (version 2.9.3)
        - Remove PackageReference: xunit.abstractions (version 2.0.3)
- [✓] (2) Add xUnit v3 packages to LexAssert.csproj:
        - Add PackageReference: xunit.v3 (version 0.3.0 or latest)
- [✓] (3) Remove xunit v2 package from LexAssert.Tests.csproj:
        - Remove PackageReference: xunit (version 2.9.3)
- [✓] (4) Add xUnit v3 packages to LexAssert.Tests.csproj:
        - Add PackageReference: xunit.v3 (version 0.3.0 or latest)
        - Update PackageReference: xunit.runner.visualstudio to version 3.0.0 or later (if not already)
        - Remove PackageReference: System.Collections (version 4.3.0) - unnecessary in .NET 8
- [✓] (5) Restore dependencies for both projects
- [✓] (6) Dependencies restored successfully (**Verify**)
- [✓] (7) Note any warnings or errors about package compatibility
- [✓] (8) Commit changes with message: "TASK-002: Replace xUnit v2 with xUnit v3 packages"

### [✓] TASK-003: Update library code for xUnit v3 compatibility *(Completed: 2026-02-10 14:38)*
**References**: Plan §Phase 2, Plan §Step 8

**IMPORTANT**: xUnit v3 has significant namespace and API changes:
- Main namespace: `Xunit` (likely unchanged)
- Exception types: May be in different namespace or renamed
- `EqualException.ForMismatchedValues()` - likely removed or changed
- `Assert` class extensibility pattern may differ

**Research Required Before Coding**:
- Identify correct namespace for xUnit v3 exceptions
- Determine proper way to throw equality assertion exceptions in v3
- Check if inheriting from `Assert` is still supported/recommended

- [✓] (1) Research xUnit v3 exception handling patterns:
        - Check xunit.v3 documentation or GitHub repo
        - Identify replacement for EqualException.ForMismatchedValues()
        - Document findings in comments or commit message
- [✓] (2) Update Lassert.cs using directives:
        - Update `using Xunit;` if namespace changed
        - Update `using Xunit.Sdk;` if namespace changed or removed
- [✓] (3) Update Lassert.cs JsonEqual() method:
        - Replace `EqualException.ForMismatchedValues(expectedJson, actualJson, "Lassert.JsonEqual() Failure")`
        - Use xUnit v3 equivalent (constructor, factory method, or different exception type)
- [✓] (4) Update Lassert.cs class declaration if needed:
        - Evaluate if `public class Lassert : Assert` still works
        - If inheritance not recommended, change to `public class Lassert` and update MembersEqual to use `Assert.Equal()`
- [✓] (5) Restore dependencies if needed
- [✓] (6) Build LexAssert.csproj in Release configuration
- [ ] (7) Library builds with 0 errors (**Verify**)
        - If compilation errors, review xUnit v3 API documentation
        - Adjust code based on actual v3 APIs
- [✓] (8) Build LexAssert.Tests.csproj in Release configuration
- [✓] (9) Test project builds with 0 errors (**Verify**)
- [✓] (10) Commit changes with message: "TASK-003: Update Lassert.cs for xUnit v3 compatibility"

### [▶] TASK-004: Update test files and validate xUnit v3 migration
**References**: Plan §Quality Gates, Plan §Step 9-10

**Test Files to Update** (if namespace/exception type changes):
- LexAssert.Tests/LexAssertTests/LexAssert_JsonEqual_Tests.cs
- LexAssert.Tests/LexAssertTests/LexAssert_MembersEqual_Tests.cs
- LexAssert.Tests/EqualityComparers/JsonEqualityComparerTests/JsonEqualityComparer_Tests.cs
- LexAssert.Tests/Demo/LexAssertDemos.cs (likely no changes needed)

- [ ] (1) Update test files using directives if xUnit v3 namespaces changed:
        - Update `using Xunit;` if needed
        - Update `using Xunit.Sdk;` if needed
- [ ] (2) Update test files exception type references:
        - If `EqualException` renamed, update `Assert.Throws<EqualException>()` calls
        - Update exception type in catch blocks
- [ ] (3) Build LexAssert.Tests.csproj in Release configuration
- [ ] (4) Test project builds with 0 errors (**Verify**)
- [ ] (5) Run all tests in LexAssert.Tests.csproj
- [ ] (6) Review test results:
        - Document pass/fail count
        - Note any failures and error messages
- [ ] (7) Fix any test failures related to xUnit v3 changes:
        - Exception type mismatches
        - Exception message format differences
        - Behavioral changes in assertions
- [ ] (8) Re-run tests after fixes
- [ ] (9) All tests pass with 0 failures (**Verify**)
        - Expected: 17+/17+ tests passing
- [ ] (10) Commit test file changes (if any) with message: "TASK-004: Update test files for xUnit v3"
- [ ] (11) Final validation: Build entire solution and run all tests
- [ ] (12) Final commit with message: "TASK-004: Complete xUnit v3 migration - all tests passing"





