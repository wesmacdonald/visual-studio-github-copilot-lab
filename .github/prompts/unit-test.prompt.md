---
mode: agent
description: Generate comprehensive unit tests using MSTest
---

# Unit Test Generator

Generate unit tests for the selected code using the MSTest framework.

## Requirements

- Use MSTest attributes ([TestClass], [TestMethod], [DataRow])
- Follow the Arrange-Act-Assert pattern
- Include both positive and negative test cases
- Test edge cases and boundary conditions
- Use descriptive test method names that explain what is being tested
- Mock dependencies where appropriate

## Test Structure

Each test should:
1. Have a clear name following the pattern: `MethodName_Scenario_ExpectedBehavior`
2. Include a brief comment explaining the test purpose
3. Use proper assertions with meaningful failure messages

## Context

Generate tests for: ${input:Describe what you want to test}

