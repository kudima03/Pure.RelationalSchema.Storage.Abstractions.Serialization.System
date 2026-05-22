# Pure.RelationalSchema.Storage.Abstractions.Serialization.System

`System.Text.Json` converters for relational schema storage abstractions in the **Pure** ecosystem.

[![.NET build & test](https://github.com/kudima03/Pure.RelationalSchema.Storage.Abstractions.Serialization.System/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.RelationalSchema.Storage.Abstractions.Serialization.System/actions/workflows/build-and-test.yml)
[![Build and Deploy](https://github.com/kudima03/Pure.RelationalSchema.Storage.Abstractions.Serialization.System/actions/workflows/publish-nuget.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.RelationalSchema.Storage.Abstractions.Serialization.System/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/Pure.RelationalSchema.Storage.Abstractions.Serialization.System)](https://www.nuget.org/packages/Pure.RelationalSchema.Storage.Abstractions.Serialization.System)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Overview

`Pure.RelationalSchema.Storage.Abstractions.Serialization.System` provides a set of `System.Text.Json` converters that enable round-trip serialization of the core storage domain types from `Pure.RelationalSchema.Storage` — cells, rows, table data sets, and schema data sets.

The entry point is `RelationalSchemaStorageConverters`, an enumerable facade that yields all four converters so they can be added to `JsonSerializerOptions` in a single loop.

## Converters

| Type | Converts | Description |
|---|---|---|
| `RelationalSchemaStorageConverters` | — | Enumerable facade; yields all four converters |
| `CellConverter` | `ICell` | Serializes/deserializes a cell value as a JSON object with a single `value` string field |
| `RowConverter` | `IRow` | Serializes/deserializes a row as an array of column–cell pairs |
| `StoredTableDataSetConverter` | `IStoredTableDataSet` | Serializes/deserializes a table data set; deserializes into an `IStoredTableDataSet` / `IQueryable<IRow>` implementation |
| `StoredSchemaDataSetConverter` | `IStoredSchemaDataSet` | Serializes/deserializes a full schema data set containing multiple tables |

## Dependencies

- [`Pure.RelationalSchema.Storage`](https://github.com/kudima03/Pure.RelationalSchema.Storage/tree/0.1.0-preview.7.0.0) — concrete implementations (`Cell`, `Row`, `StoredSchemaDataset`) and their interfaces (`ICell`, `IRow`, `IStoredTableDataSet`, `IStoredSchemaDataSet`)
- [`Pure.RelationalSchema.Storage.HashCodes`](https://github.com/kudima03/Pure.RelationalSchema.Storage.HashCodes/tree/0.1.0-preview.2.1.0) — hash code implementations used as dictionary keys for columns and tables during deserialization
- [`Pure.Collections.Generic`](https://github.com/kudima03/Pure.Collections.Generic/tree/0.1.0-preview.3.0.0) — generic dictionary with custom equality comparer support used when reconstructing rows and schema data sets

## Target Frameworks

- .NET 8
- .NET 9
- .NET 10

## Installation

```shell
dotnet add package Pure.RelationalSchema.Storage.Abstractions.Serialization.System
```

## Usage

```csharp
var options = new JsonSerializerOptions();

foreach (JsonConverter converter in new RelationalSchemaStorageConverters())
    options.Converters.Add(converter);

// Serialize
string json = JsonSerializer.Serialize(storedSchema, options);

// Deserialize
IStoredSchemaDataSet restored =
    JsonSerializer.Deserialize<IStoredSchemaDataSet>(json, options)!;
```
