# ExperimentFramework.Generators

Source generator for ExperimentFramework that generates compile-time proxies for high-performance experiment execution.

## What it does

This package contains a Roslyn source generator that automatically generates proxy classes for your experiment interfaces at compile time. These generated proxies eliminate reflection overhead and provide near-zero-cost abstraction for A/B testing and experimentation.

## Installation

```bash
dotnet add package ExperimentFramework
dotnet add package ExperimentFramework.Generators
```

## Usage

The generator runs automatically when you build your project. No additional configuration is needed beyond defining your experiments using the `ExperimentFrameworkBuilder`.

For complete documentation, see the [ExperimentFramework GitHub repository](https://github.com/anthropics/ExperimentFramework).

## Requirements

- .NET 8.0 or later
- C# 12 or later

## License

MIT
