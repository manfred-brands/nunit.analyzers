# NUnit1029

## Console Access in fixture.

| Topic    | Value
| :--      | :--
| Id       | NUnit1029
| Severity | Info
| Enabled  | True
| Category | Structure
| Code     | [ConsoleAccessAnalyzer](https://github.com/nunit/nunit.analyzers/blob/master/src/nunit.analyzers/ConsoleAccess/ConsoleAccessAnalyzer.cs)

## Description

A fixture should not access the Console.

## Motivation

Console output will slow down testing and pollute continuous integration logs.

## How to fix violations

Remove any access to System.Console class. Use the message parameter to NUnit Assert for failure messages.

<!-- start generated config severity -->
## Configure severity

### Via ruleset file

Configure the severity per project, for more info see [MSDN](https://msdn.microsoft.com/en-us/library/dd264949.aspx).

### Via .editorconfig file

```ini
# NUnit1029: Console Access in fixture.
dotnet_diagnostic.NUnit1029.severity = chosenSeverity
```

where `chosenSeverity` can be one of `none`, `silent`, `suggestion`, `warning`, or `error`.

### Via #pragma directive

```csharp
#pragma warning disable NUnit1029 // Console Access in fixture.
Code violating the rule here
#pragma warning restore NUnit1029 // Console Access in fixture.
```

Or put this at the top of the file to disable all instances.

```csharp
#pragma warning disable NUnit1029 // Console Access in fixture.
```

### Via attribute `[SuppressMessage]`

```csharp
[System.Diagnostics.CodeAnalysis.SuppressMessage("Structure",
    "NUnit1029:Console Access in fixture.",
    Justification = "Reason...")]
```
<!-- end generated config severity -->
