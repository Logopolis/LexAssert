# Migration Plan: xUnit v2 to xUnit v3 Upgrade

**Date**: January 2025  
**Repository**: C:\Users\ChrisSegrove.AzureAD\repos\LexAssert  
**Solution**: LexAssert.sln  
**Plan Version**: 1.0  
**Based on Assessment**: assessment.md

---

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [Migration Strategy](#migration-strategy)
3. [Detailed Dependency Analysis](#detailed-dependency-analysis)
4. [Project-by-Project Plans](#project-by-project-plans)
   - [LexAssert.csproj (Library)](#lexassertcsproj-library)
   - [LexAssert.Tests.csproj (Test Project)](#lexasserttestscsproj-test-project)
5. [Risk Management](#risk-management)
6. [Testing & Validation Strategy](#testing--validation-strategy)
7. [Complexity & Effort Assessment](#complexity--effort-assessment)
8. [Source Control Strategy](#source-control-strategy)
9. [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Overview

LexAssert is a NuGet package that extends xUnit's assertion capabilities with JSON-based equality testing. This plan guides the migration from xUnit v2 (deprecated) to xUnit v3 (current), addressing breaking changes in exception handling APIs and inheritance patterns.

**Scope**: 
- 2 projects (1 library targeting .NET Standard 2.0, 1 test project targeting .NET 8.0)
- 2 public API classes: `Lassert` and `JsonEqualityComparer<T>`
- ~17 test methods providing comprehensive coverage
- NuGet package with external consumers (breaking change impact)

### Discovered Metrics

**Solution Characteristics**:
- Total projects: 2
- Dependency depth: 1 level (test ? library)
- No circular dependencies
- xUnit v2 package references: 3 (xunit 2.9.3, xunit.abstractions 2.0.3, xunit.runner.visualstudio 3.1.1)

**Risk Indicators**:
- **CRITICAL DISCOVERY**: xUnit v3 requires .NET 8+ or .NET Framework 4.7.2+ (does NOT support .NET Standard 2.0)
- **Framework upgrade required**: Library must upgrade from .NET Standard 2.0 to .NET 8.0 first
- **Critical API breaking change**: `EqualException.ForMismatchedValues()` no longer exists in xUnit v3
- **Design pattern change**: Inheritance from `Assert` class needs evaluation
- **Consumer impact**: All package consumers must upgrade to xUnit v3 AND .NET 8+

**Code Metrics**:
- Files requiring code changes: 4 (1 library file, 3 test files)
- xUnit API usage locations: 8+ EqualException references, 1 Assert inheritance
- Test coverage: Comprehensive (17+ tests covering all public APIs)
- **Framework changes required**: 1 project file (.NET Standard 2.0 ? .NET 8.0)

### Complexity Classification

**Classification: Medium Complexity**

**Justification**:
- ? Small project count (2 projects)
- ? Simple dependency structure (single level)
- ?? **Framework upgrade required** (.NET Standard 2.0 ? .NET 8.0) - MAJOR breaking change
- ?? Critical API breaking changes requiring research
- ?? Consumer breaking change requiring versioning strategy
- ?? **Drops support for .NET Framework < 4.7.2** - impacts consumer compatibility
- ? Excellent test coverage for validation

This is manageable due to small scope, but the **mandatory framework upgrade** makes this a **larger breaking change** for consumers than initially anticipated.

### Selected Strategy

**All-At-Once Strategy** (with two-stage execution)

**Rationale**:
- Only 2 projects with simple dependency relationship
- Both projects use xUnit equally (no incremental benefit)
- xUnit v3 package updates must be coordinated across solution
- Library changes and test updates are tightly coupled
- Small enough to validate comprehensively in single operation
- **Framework upgrade and xUnit upgrade can be done in sequence**

**Two-Stage Approach** (due to framework compatibility requirement):

**Stage 1: Framework Upgrade**
1. Upgrade LexAssert.csproj: .NET Standard 2.0 ? .NET 8.0
2. Build and test to ensure no framework-related breaks
3. Update package metadata for framework change

**Stage 2: xUnit v3 Upgrade**
1. Research xUnit v3 APIs (exception handling patterns)
2. Update both projects to xUnit v3 packages
3. Update code for xUnit v3 compatibility
4. Comprehensive testing and validation

**Combined Impact**: Version 3.0.0 with BOTH framework and xUnit upgrades

### Expected Remaining Iterations

Based on medium complexity classification:
- **Iteration 2.1**: Dependency Analysis (foundation)
- **Iteration 2.2**: Migration Strategy Details (foundation)
- **Iteration 2.3**: Project Stubs & Risk/Complexity Overview (foundation)
- **Iteration 3.1**: Research Phase Plan (prerequisite work)
- **Iteration 3.2**: Library Project Detailed Plan
- **Iteration 3.3**: Test Project Detailed Plan
- **Iteration 3.4**: Documentation & Consumer Communication Plan
- **Final Iteration**: Success Criteria and Source Control Strategy

Total: ~8 iterations to complete comprehensive plan.

---

## Migration Strategy

### Approach Selection

**Selected: All-At-Once Strategy**

### Justification for All-At-Once

**Why Not Incremental?**
- Only 2 projects - no benefit from phasing
- Library and tests are tightly coupled; cannot test library without upgrading tests
- xUnit package versions must be consistent across solution (cannot mix v2 and v3)
- Test project has no independent value without library project
- Research findings will apply to both projects equally

**Why All-At-Once?**
- ? Small solution (2 projects) - manageable as single unit
- ? Simple dependency structure (no complex graph to navigate)
- ? Excellent test coverage - comprehensive validation in single pass
- ? Faster completion - single research phase, single validation cycle
- ? Cleaner version control - single atomic commit for entire upgrade
- ? Easier rollback - single commit to revert if needed

### All-At-Once Strategy Rationale

**Atomic Operation Characteristics**:
- All package references updated in single operation
- All code changes applied together
- Single build to identify all compilation issues
- Single test run to validate all behavior
- One commit for entire upgrade

**Risk Management**:
- Comprehensive test coverage provides safety net
- Small scope reduces coordination complexity
- Clean git state enables easy rollback
- Research phase mitigates API uncertainty

### Dependency-Based Ordering

While using All-At-Once strategy, operations will still follow dependency order within the atomic upgrade:

**Ordering Principles**:
1. **Library project first** (foundation layer):
   - Update LexAssert.csproj package references
   - Modify Lassert.cs code
   - Build library project to identify compilation issues

2. **Test project second** (consumer layer):
   - Update LexAssert.Tests.csproj package references
   - Modify test files
   - Build test project

3. **Solution validation third** (integration):
   - Build entire solution
   - Run all tests
   - Verify behavior parity

This ordering ensures:
- Compilation errors in library are caught before test changes
- Test changes can reference corrected library code
- Dependency direction is respected (bottom-up)

### Parallel vs Sequential Execution

**Decision: Sequential Execution**

**Rationale**:
- Library changes inform test changes (exception types, API patterns)
- Cannot validate test changes until library compiles
- Single developer scenario (no parallelization benefit)
- Sequential approach reduces cognitive load

**Execution Sequence**:
1. Research xUnit v3 APIs (prerequisite)
2. Update library project (foundation)
3. Update test project (consumer)
4. Build and validate (integration)

### Phase Definitions

#### Phase 0: Research & Prerequisites

**Purpose**: Identify xUnit v3 API replacements and verify framework compatibility.

**Deliverables**:
- ? **Confirmed**: xUnit v3 requires .NET 8+ or .NET Framework 4.7.2+ (does NOT support .NET Standard 2.0)
- **Decision**: Upgrade library to .NET 8.0 (modern .NET, aligns with test project)
- Confirmed xUnit v3 package names and versions
- Identified replacement for `EqualException.ForMismatchedValues()`
- Documented any xUnit v3 best practices for Assert extensibility

**Success Criteria**:
- ? Framework upgrade path determined (.NET Standard 2.0 ? .NET 8.0)
- Clear understanding of how to throw assertion exceptions in xUnit v3
- List of all package updates needed
- Consumer impact understood and documented

**Estimated Effort**: Medium (requires external research, may need source code review)

**CRITICAL FINDING**: xUnit v3 does not support .NET Standard 2.0, requiring framework upgrade before xUnit upgrade.

---

#### Phase 1: Framework Upgrade (.NET Standard 2.0 ? .NET 8.0)

**Purpose**: Upgrade library project to .NET 8.0 to meet xUnit v3 requirements.

**Operations**:

1. **Update LexAssert.csproj target framework**:
   ```xml
   <!-- Before -->
   <TargetFramework>netstandard2.0</TargetFramework>
   
   <!-- After -->
   <TargetFramework>net8.0</TargetFramework>
   ```

2. **Update package metadata** (breaking change communication):
   ```xml
   <VersionPrefix>3.0.0</VersionPrefix>
   <PackageReleaseNotes>BREAKING CHANGE: Upgraded to .NET 8.0 and xUnit v3. Minimum requirements: .NET 8+ or .NET Framework 4.7.2+. See README for migration guide.</PackageReleaseNotes>
   ```

3. **Build library project**:
   - Verify no compilation errors
   - Check for any framework compatibility issues
   - Review warnings

4. **Run tests** (still using xUnit v2 at this stage):
   - All tests should still pass
   - Validates framework change doesn't break functionality

**Deliverables**:
- Library project targets .NET 8.0
- All tests pass with new framework
- No functional regressions

**Success Criteria**:
- `dotnet build LexAssert/LexAssert.csproj` succeeds
- `dotnet test LexAssert.Tests/LexAssert.Tests.csproj` shows 100% pass rate
- No unexpected warnings or errors

**Consumer Impact**:
- **BREAKING**: Drops support for .NET Framework < 4.7.2
- **BREAKING**: Drops support for .NET Core 2.x, .NET 5, .NET 6, .NET 7
- Consumers must be on .NET 8+ or .NET Framework 4.7.2+

**Estimated Effort**: Low (straightforward framework change, no code changes expected)

---

## Project-by-Project Plans

### PREREQUISITE: Framework Upgrade Required

**CRITICAL DISCOVERY**: xUnit v3 requires .NET 8+ or .NET Framework 4.7.2+. The library project must upgrade from .NET Standard 2.0 to .NET 8.0 **BEFORE** upgrading to xUnit v3.

**Decision**: Upgrade library to .NET 8.0
- Aligns with test project (already .NET 8.0)
- Modern .NET with best performance
- Simplifies solution (both projects on same framework)
- Required for xUnit v3 compatibility

---

### LexAssert.csproj (Library)

**Current State**:
- **Target Framework**: .NET Standard 2.0
- **xUnit Version**: v2.9.3
- **Key Dependencies**: 
  - xunit 2.9.3
  - xunit.abstractions 2.0.3
  - System.Text.Json 9.0.6
- **Public API**: 
  - `Lassert` class (inherits from `Assert`)
  - `JsonEqualityComparer<T>` class
- **Critical Code**: Lassert.cs uses `EqualException.ForMismatchedValues()`

**Target State**:
- **Target Framework**: **.NET 8.0** (upgraded from .NET Standard 2.0)
- **xUnit Version**: v3.x (latest stable)
- **Updated Dependencies**: 
  - xunit v3.x (TBD exact version)
  - Remove xunit.abstractions (if obsolete in v3)
  - Add xunit.assert or xunit.core if needed (v3 package structure)
  - System.Text.Json 9.0.6 (no change)
- **Public API**: Same public surface, internal implementation updated

---

#### PHASE 1: Framework Upgrade (.NET Standard 2.0 ? .NET 8.0)

This phase must complete successfully BEFORE proceeding to xUnit v3 upgrade.

**Step 1: Update Target Framework**

**File**: `LexAssert/LexAssert.csproj`

**Change**:
```xml
<!-- BEFORE -->
<TargetFramework>netstandard2.0</TargetFramework>

<!-- AFTER -->
<TargetFramework>net8.0</TargetFramework>
```

**Actions**:
1. Open `LexAssert/LexAssert.csproj` in editor
2. Locate `<TargetFramework>netstandard2.0</TargetFramework>` line
3. Change to `<TargetFramework>net8.0</TargetFramework>`
4. Save file

**Expected Impact**: 
- Library now targets .NET 8.0
- No code changes required (framework upgrade should be transparent)
- System.Text.Json package may benefit from framework-included version

---

**Step 2: Update Package Metadata (Version Bump)**

**File**: `LexAssert/LexAssert.csproj`

**Change**:
```xml
<!-- BEFORE -->
<VersionPrefix>2.2.1</VersionPrefix>
<PackageReleaseNotes>Update dependencies and remove stale ones</PackageReleaseNotes>

<!-- AFTER -->
<VersionPrefix>3.0.0</VersionPrefix>
<PackageReleaseNotes>BREAKING CHANGE: Upgraded to .NET 8.0 and xUnit v3.
Minimum requirements: .NET 8+ or .NET Framework 4.7.2+.
Public API unchanged. See README for migration guide.</PackageReleaseNotes>
```

**Rationale**:
- Major version bump (3.0.0) signals breaking change
- Clear communication of framework requirement
- Consumers understand this is not backward compatible

---

**Step 3: Restore Packages**

**Command**:
```bash
dotnet restore LexAssert/LexAssert.csproj
```

**Expected Outcome**:
- Packages restore successfully
- No errors about missing framework
- System.Text.Json resolves correctly for .NET 8.0

**Potential Issues**:
- System.Text.Json may have warnings (can ignore - v9.0.6 supports .NET 8.0)
- No other issues expected

---

**Step 4: Build Library Project**

**Command**:
```bash
dotnet build LexAssert/LexAssert.csproj --configuration Release
```

**Expected Outcome**:
- Build succeeds with 0 errors
- May have 0 warnings (clean build expected)
- Library DLL targets .NET 8.0

**Potential Issues**:
- **Unlikely**: Framework API differences between .NET Standard 2.0 and .NET 8.0
- **If errors occur**: Review and address framework-specific issues
- **Most likely**: Clean build (library code is simple)

---

**Step 5: Build and Run Tests (Framework Validation)**

**Commands**:
```bash
dotnet build LexAssert.Tests/LexAssert.Tests.csproj --configuration Release
dotnet test LexAssert.Tests/LexAssert.Tests.csproj --configuration Release
```

**Expected Outcome**:
- Tests build successfully
- **All 17+ tests PASS** (100% pass rate)
- Validates framework upgrade doesn't break functionality
- Still using xUnit v2 at this stage

**Success Criteria for Phase 1**:
- [ ] Library project builds with 0 errors on .NET 8.0
- [ ] Test project builds with 0 errors
- [ ] All tests pass (17+/17+)
- [ ] No functional regressions from framework change
- [ ] Package metadata updated for v3.0.0

**If Phase 1 fails**: 
- Address framework-specific issues before proceeding
- DO NOT proceed to Phase 2 (xUnit v3 upgrade) until all Phase 1 criteria met

---

#### PHASE 2: xUnit v3 Upgrade (After Framework Upgrade)



**Step 6: Research xUnit v3 APIs**

**Deliverables**:
- Identify new exception handling patterns in xUnit v3
- Determine if `Lassert` class should still inherit from `Assert`
- Update documentation with xUnit v3 migration notes

**Success Criteria**:
- Clear plan for updating code to use xUnit v3 APIs
- Documentation updated with any new best practices

---

**Step 7: Update Package References to xUnit v3**

**Files**:
- `LexAssert/LexAssert.csproj`
- `LexAssert.Tests/LexAssert.Tests.csproj`

**Changes**:
```xml
<!-- Library project -->
<PackageReference Include="xunit" Version="3.0.0" />
<PackageReference Remove="xunit.abstractions" />
<!-- Test project -->
<PackageReference Include="xunit" Version="3.0.0" />
```

**Actions**:
1. Open both `csproj` files in editor
2. Update xUnit package references to v3.0.0
3. Remove `xunit.abstractions` package reference (obsolete in v3)
4. Save files

**Expected Impact**: 
- Both projects use xUnit v3
- Obsolete package references removed

---

**Step 8: Update Library Code for xUnit v3**

**File**: `Lassert.cs`

**Changes**:
- Replace `EqualException.ForMismatchedValues()` usage with xUnit v3 equivalent
  - Research and use `Assert.Equal()` or custom assertion strategy
- Update using directives for namespace changes (if any)
- Remove Assert inheritance if no longer needed

**Actions**:
1. Open `Lassert.cs` in editor
2. Refactor code to use xUnit v3 APIs
3. Update or remove `Assert` inheritance as decided
4. Save file

**Expected Impact**:
- Library code is compatible with xUnit v3
- May involve design pattern change (composition over inheritance)

---

**Step 9: Update Test Code for xUnit v3**

**Files**: All test files in `LexAssert.Tests` project

**Changes**:
- Update exception type references in tests
- Update using directives for namespace changes
- Fix test assertion patterns to align with xUnit v3

**Actions**:
1. Open each test file in editor
2. Update code to be compatible with xUnit v3
3. Save files

**Expected Impact**:
- Test code is compatible with xUnit v3
- All tests should continue to pass

---

**Step 10: Documentation Updates**

**Files**:
- README.md
- Migration guide (if exists)

**Changes**:
- Update README with new .NET 8+ and xUnit v3 requirements
- Add comprehensive migration guide for consumers
- Update package metadata in `csproj` files

**Actions**:
1. Update README and migration guide documentation
2. Ensure all references to framework and xUnit versions are current
3. Save and commit documentation changes

**Expected Impact**:
- Consumers have clear guidance on migration
- Documentation reflects current state of project

---

### Quality Gates

**Gate 0: Research Phase** ? **COMPLETED**
- [x] xUnit v3 framework requirements identified
- [x] Decision made: Upgrade to .NET 8.0
- [x] Two-stage approach defined

**Gate 1: Framework Upgrade** ?
Cannot proceed to xUnit v3 upgrade until:
- [ ] LexAssert.csproj targets .NET 8.0
- [ ] Library builds without errors
- [ ] All tests pass (100%)
- [ ] No framework-related regressions
- [ ] Package version bumped to 3.0.0

---

**Gate 2: xUnit v3 Package Updates** ?
Cannot proceed to code changes until:
- [ ] xUnit v3 exception API identified
- [ ] Package structure understood
- [ ] Both projects reference xUnit v3
- [ ] Packages restore successfully

---

**Gate 3: Library Compilation** ?
Cannot proceed to test project until:
- [ ] LexAssert.csproj builds without errors
- [ ] No obsolete API warnings
- [ ] Exception throwing code verified

---

**Gate 4: Test Validation** ?
Cannot proceed to commit until:
- [ ] All tests pass (100%)
- [ ] Exception behavior validated
- [ ] No regressions detected

---

**Gate 5: Documentation** ?
Cannot proceed to release until:
- [ ] Version confirmed at 3.0.0
- [ ] README updated with BOTH framework and xUnit requirements
- [ ] Migration guide complete
- [ ] Release notes prepared

---

**Gate 6: Release Approval** ?
Cannot publish to NuGet until:
- [ ] All quality gates passed
- [ ] Peer review completed (if applicable)
- [ ] Git tagged appropriately
- [ ] v2.x maintenance branch created

### Commit Strategy

**Recommended Approach**: Sequential commits on feature branch (reflecting two-stage approach)

**Commit Sequence**:

**Commit 1: Research findings documentation**
```bash
git commit -m "docs: Document xUnit v3 research findings

- Confirmed xUnit v3 requires .NET 8+ or .NET Framework 4.7.2+
- xUnit v3 does NOT support .NET Standard 2.0
- Decision: Upgrade library to .NET 8.0
- Documented framework upgrade as prerequisite
- Identified two-stage migration approach"
```

**Commit 2: Framework upgrade (.NET Standard 2.0 ? .NET 8.0)**
```bash
git commit -m "build: Upgrade LexAssert to .NET 8.0

- Update TargetFramework: netstandard2.0 -> net8.0
- Bump version to 3.0.0
- Update package release notes for breaking change
- All tests pass with new framework

BREAKING CHANGE: Requires .NET 8+ or .NET Framework 4.7.2+"
```

**Commit 3: xUnit v3 package updates (both projects)**
```bash
git commit -m "build: Update both projects to xUnit v3

- xunit 2.9.3 -> 3.0.0 (library)
- xunit 2.9.3 -> 3.0.0 (test project)
- Remove xunit.abstractions (obsolete in v3)
- Remove System.Collections 4.3.0 (unnecessary)"
```

**Commit 4: Library code changes for xUnit v3**
```bash
git commit -m "refactor: Update Lassert.cs for xUnit v3 compatibility

- Replace EqualException.ForMismatchedValues() with v3 API
- Update using directives for namespace changes
- [Refactor to composition if needed]

BREAKING CHANGE: Requires xUnit v3"
```

**Commit 5: Test code changes for xUnit v3**
```bash
git commit -m "test: Update test files for xUnit v3

- Update exception type references
- Update using directives
- Fix test assertion patterns for v3"
```

**Commit 6: Documentation updates**
```bash
git commit -m "docs: Update documentation for v3 release

- Update README with .NET 8+ requirement
- Update README with xUnit v3 requirement
- Add comprehensive migration guide
- Update package metadata

BREAKING CHANGE: LexAssert 3.0 requires .NET 8+ and xUnit v3"
```

**Alternative: Two Atomic Commits** (Stage-based)

**Commit 1: Framework Upgrade (Complete)**
```bash
git add -A
git commit -m "feat: Upgrade to .NET 8.0

Complete framework migration from .NET Standard 2.0 to .NET 8.0

BREAKING CHANGE: Requires .NET 8+ or .NET Framework 4.7.2+

- Updated target framework
- Bumped version to 3.0.0
- All tests pass
- No code changes required"
```

**Commit 2: xUnit v3 Upgrade (Complete)**
```bash
git add -A
git commit -m "feat: Upgrade to xUnit v3

Complete migration from xUnit v2.9.3 to xUnit v3.0.0

BREAKING CHANGE: Requires xUnit v3

- Updated xUnit packages
- Updated exception handling code
- Updated test assertions
- Updated documentation
- All tests pass"
```

**Recommendation**: **Sequential commits** (6 commits)
- Clearer progression through stages
- Easier to review each change
- Better git history for future reference
- Aligns with two-stage validation approach
- **Commit 2 can be validated independently** (framework upgrade complete before xUnit)
