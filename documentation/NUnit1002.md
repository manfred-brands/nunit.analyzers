# NUnit1002
## Find TestCaseSource(StringConstant) Usage

| Topic    | Value
| :--      | :--
| Id       | NUnit1002
| Severity | Warning
| Enabled  | True
| Category | Structure
| Code     | [TestCaseSourceUsesStringAnalyzer](https://github.com/nunit/nunit.analyzers/blob/master/src/nunit.analyzers/TestCaseSourceUsage/TestCaseSourceUsesStringAnalyzer.cs)


## Description



## Motivation

ADD MOTIVATION HERE

## How to fix violations

ADD HOW TO FIX VIOLATIONS HERE

<!-- start generated config severity -->
## Configure severity

### Via ruleset file.

Configure the severity per project, for more info see [MSDN](https://msdn.microsoft.com/en-us/library/dd264949.aspx).

### Via #pragma directive.
```C#
#pragma warning disable NUnit1002 // Find TestCaseSource(StringConstant) Usage
Code violating the rule here
#pragma warning restore NUnit1002 // Find TestCaseSource(StringConstant) Usage
```

Or put this at the top of the file to disable all instances.
```C#
#pragma warning disable NUnit1002 // Find TestCaseSource(StringConstant) Usage
```

### Via attribute `[SuppressMessage]`.

```C#
[System.Diagnostics.CodeAnalysis.SuppressMessage("Structure", 
    "NUnit1002:Find TestCaseSource(StringConstant) Usage", 
    Justification = "Reason...")]
```
<!-- end generated config severity -->