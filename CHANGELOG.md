# Changelog

All notable changes to Pure.RelationalSchema.Storage.Abstractions.Serialization.System are documented here.

Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

---

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
