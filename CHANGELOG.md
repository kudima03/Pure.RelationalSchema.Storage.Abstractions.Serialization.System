# Changelog

All notable changes to Pure.RelationalSchema.Storage.Abstractions.Serialization.System are documented here.

Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

---

## [0.1.0-preview.1.2.0] — 2026-11-11

### Added

- Added `net11.0` as a target framework.

### Changed

- Updated dependencies to their .NET 11 releases: `Pure.Primitives` 3.8.0,
  `Pure.RelationalSchema.Storage` 0.1.0-preview.8.1.0,
  `Pure.RelationalSchema.Storage.HashCodes` 0.1.0-preview.2.3.0 and
  `Pure.Collections.Generic` 0.1.0-preview.3.1.0.
- Build and release tooling updates (CI and devcontainer moved to .NET 11).

## [0.1.0-preview.1.1.0] — 2026-10-22

### Changed

- Updated `Pure.Primitives` dependency from 3.6.5 to 3.7.0.
- The NuGet package now includes `README.md`.
- Assembly version is now derived from the release tag (`0.0.0.0` for the
  `0.x` series); no public API changes.
- Build and release tooling updates (NuGet trusted publishing, `.slnx`
  solution format, local tool manifests, devcontainer image).

## [0.1.0-preview.1.0.1] — 2026-06-25

- Maintenance release: dependency and build updates.

## [0.1.0-preview.1.0.0] — 2026-06-07

- Maintenance release: dependency and build updates.

## [0.1.0-preview.0.2.1] — 2026-03-10

### Changed

- **`CellConverter`** now serializes `ICell` as a JSON object with a single
  `value` string field instead of serializing the concrete `Cell` type
  directly.
- **`RowConverter`** now serializes each cell as a `{ "column": ..., "cell": ... }`
  object instead of a bare key/value pair, fixing round-trip
  deserialization of rows.
- **`StoredSchemaDataSetConverter`** — **Breaking:** the `tables` property
  in the JSON representation of a schema data set was renamed to
  `datasets`.

## [0.1.0-preview.0.2.0] — 2026-02-26

### Added

- **`RelationalSchemaStorageConverters`** — enumerable facade yielding
  `CellConverter`, `RowConverter`, `StoredTableDataSetConverter`, and
  `StoredSchemaDataSetConverter` so all four can be registered on
  `JsonSerializerOptions` in a single loop.

## [0.1.0-preview.0.1.0] — 2026-02-25

### Added

- **`CellConverter`** — serializes/deserializes `ICell`.
- **`RowConverter`** — serializes/deserializes `IRow` as an array of
  column–cell pairs.
- **`StoredTableDataSetConverter`** — serializes/deserializes
  `IStoredTableDataSet`, reconstructing an `IQueryable<IRow>`
  implementation on deserialization.
- **`StoredSchemaDataSetConverter`** — serializes/deserializes
  `IStoredSchemaDataSet` containing multiple tables.
