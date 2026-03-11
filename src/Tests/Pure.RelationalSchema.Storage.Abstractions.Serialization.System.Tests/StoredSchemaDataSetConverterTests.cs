using System.Text.Json;
using System.Text.Json.Serialization;
using Pure.Primitives.Abstractions.Serialization.System;
using Pure.Primitives.Abstractions.String;
using Pure.Primitives.Random.String;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.Abstractions.Serialization.System;
using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.ColumnType;
using Pure.RelationalSchema.HashCodes;
using Pure.RelationalSchema.Storage.Abstractions.Serialization.System.Tests.Fakes;
using Pure.RelationalSchema.Storage.HashCodes;
using Char = Pure.Primitives.Char.Char;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System.Tests;

public sealed record StoredSchemaDataSetConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StoredSchemaDataSetConverterTests()
    {
        _options = new JsonSerializerOptions();

        foreach (JsonConverter converter in new PrimitiveConverters())
        {
            _options.Converters.Add(converter);
        }

        foreach (JsonConverter converter in new RelationalSchemaConverters())
        {
            _options.Converters.Add(converter);
        }

        foreach (JsonConverter converter in new RelationalSchemaStorageConverters())
        {
            _options.Converters.Add(converter);
        }

        _options.WriteIndented = true;
        _options.NewLine = "\n";
    }

    [Fact]
    public void Write()
    {
        IString columnName1 = new RandomString(new Char('a'), new Char('z'));

        IString columnName2 = new RandomString(new Char('a'), new Char('z'));

        IString cellValue1 = new RandomString(new Char('a'), new Char('z'));

        IString cellValue2 = new RandomString(new Char('a'), new Char('z'));

        IString tableName = new RandomString(new Char('a'), new Char('z'));

        IString schemaName = new RandomString(new Char('a'), new Char('z'));

        IStoredSchemaDataSet schemaDataset = new StoredSchemaDataset(
            new Schema.Schema(
                schemaName,
                [
                    new Table.Table(
                        tableName,
                        [
                            new Column.Column(columnName1, new StringColumnType()),
                            new Column.Column(columnName2, new StringColumnType()),
                        ],
                        []
                    ),
                ],
                []
            ),
            new Collections.Generic.Dictionary<
                KeyValuePair<ITable, IStoredTableDataSet>,
                ITable,
                IStoredTableDataSet
            >(
                [
                    new KeyValuePair<ITable, IStoredTableDataSet>(
                        new Table.Table(
                            tableName,
                            [
                                new Column.Column(columnName1, new StringColumnType()),
                                new Column.Column(columnName2, new StringColumnType()),
                            ],
                            []
                        ),
                        new FakeStoredTableDataset(
                            new Table.Table(
                                tableName,
                                [
                                    new Column.Column(
                                        columnName1,
                                        new StringColumnType()
                                    ),
                                    new Column.Column(
                                        columnName2,
                                        new StringColumnType()
                                    ),
                                ],
                                []
                            ),
                            [
                                new Row(
                                    new Collections.Generic.Dictionary<
                                        KeyValuePair<IColumn, ICell>,
                                        IColumn,
                                        ICell
                                    >(
                                        [
                                            new KeyValuePair<IColumn, ICell>(
                                                new Column.Column(
                                                    columnName1,
                                                    new StringColumnType()
                                                ),
                                                new Cell(cellValue1)
                                            ),
                                            new KeyValuePair<IColumn, ICell>(
                                                new Column.Column(
                                                    columnName2,
                                                    new StringColumnType()
                                                ),
                                                new Cell(cellValue2)
                                            ),
                                        ],
                                        x => x.Key,
                                        x => x.Value,
                                        x => new ColumnHash(x)
                                    )
                                ),
                            ]
                        )
                    ),
                ],
                x => x.Key,
                x => x.Value,
                x => new TableHash(x)
            )
        );

        string serialized = JsonSerializer.Serialize(schemaDataset, _options);

        Assert.Equal(
            $$"""
            {
              "Schema": {
                "Name": "{{schemaName.TextValue}}",
                "Tables": [
                  {
                    "Name": "{{tableName.TextValue}}",
                    "Columns": [
                      {
                        "Name": "{{columnName1.TextValue}}",
                        "Type": "string"
                      },
                      {
                        "Name": "{{columnName2.TextValue}}",
                        "Type": "string"
                      }
                    ],
                    "Indexes": []
                  }
                ],
                "ForeignKeys": []
              },
              "Datasets": [
                {
                  "TableSchema": {
                    "Name": "{{tableName.TextValue}}",
                    "Columns": [
                      {
                        "Name": "{{columnName1.TextValue}}",
                        "Type": "string"
                      },
                      {
                        "Name": "{{columnName2.TextValue}}",
                        "Type": "string"
                      }
                    ],
                    "Indexes": []
                  },
                  "Rows": [
                    [
                      {
                        "Column": {
                          "Name": "{{columnName1.TextValue}}",
                          "Type": "string"
                        },
                        "Cell": {
                          "Value": "{{cellValue1.TextValue}}"
                        }
                      },
                      {
                        "Column": {
                          "Name": "{{columnName2.TextValue}}",
                          "Type": "string"
                        },
                        "Cell": {
                          "Value": "{{cellValue2.TextValue}}"
                        }
                      }
                    ]
                  ]
                }
              ]
            }
            """,
            serialized
        );
    }

    [Fact]
    public void Read()
    {
        IString columnName1 = new RandomString(new Char('a'), new Char('z'));

        IString columnName2 = new RandomString(new Char('a'), new Char('z'));

        IString cellValue1 = new RandomString(new Char('a'), new Char('z'));

        IString cellValue2 = new RandomString(new Char('a'), new Char('z'));

        IString tableName = new RandomString(new Char('a'), new Char('z'));

        IString schemaName = new RandomString(new Char('a'), new Char('z'));

        string input = $$"""
            {
              "Schema": {
                "Name": "{{schemaName.TextValue}}",
                "Tables": [
                  {
                    "Name": "{{tableName.TextValue}}",
                    "Columns": [
                      {
                        "Name": "{{columnName1.TextValue}}",
                        "Type": "string"
                      },
                      {
                        "Name": "{{columnName2.TextValue}}",
                        "Type": "string"
                      }
                    ],
                    "Indexes": []
                  }
                ],
                "ForeignKeys": []
              },
              "Datasets": [
                {
                  "TableSchema": {
                    "Name": "{{tableName.TextValue}}",
                    "Columns": [
                      {
                        "Name": "{{columnName1.TextValue}}",
                        "Type": "string"
                      },
                      {
                        "Name": "{{columnName2.TextValue}}",
                        "Type": "string"
                      }
                    ],
                    "Indexes": []
                  },
                  "Rows": [
                    [
                      {
                        "Column": {
                          "Name": "{{columnName1.TextValue}}",
                          "Type": "string"
                        },
                        "Cell": {
                          "Value": "{{cellValue1.TextValue}}"
                        }
                      },
                      {
                        "Column": {
                          "Name": "{{columnName2.TextValue}}",
                          "Type": "string"
                        },
                        "Cell": {
                          "Value": "{{cellValue2.TextValue}}"
                        }
                      }
                    ]
                  ]
                }
              ]
            }
            """;

        Assert.True(
            new StoredSchemaDataSetHash(
                new StoredSchemaDataset(
                    new Schema.Schema(
                        schemaName,
                        [
                            new Table.Table(
                                tableName,
                                [
                                    new Column.Column(
                                        columnName1,
                                        new StringColumnType()
                                    ),
                                    new Column.Column(
                                        columnName2,
                                        new StringColumnType()
                                    ),
                                ],
                                []
                            ),
                        ],
                        []
                    ),
                    new Collections.Generic.Dictionary<
                        KeyValuePair<ITable, IStoredTableDataSet>,
                        ITable,
                        IStoredTableDataSet
                    >(
                        [
                            new KeyValuePair<ITable, IStoredTableDataSet>(
                                new Table.Table(
                                    tableName,
                                    [
                                        new Column.Column(
                                            columnName1,
                                            new StringColumnType()
                                        ),
                                        new Column.Column(
                                            columnName2,
                                            new StringColumnType()
                                        ),
                                    ],
                                    []
                                ),
                                new FakeStoredTableDataset(
                                    new Table.Table(
                                        tableName,
                                        [
                                            new Column.Column(
                                                columnName1,
                                                new StringColumnType()
                                            ),
                                            new Column.Column(
                                                columnName2,
                                                new StringColumnType()
                                            ),
                                        ],
                                        []
                                    ),
                                    [
                                        new Row(
                                            new Collections.Generic.Dictionary<
                                                KeyValuePair<IColumn, ICell>,
                                                IColumn,
                                                ICell
                                            >(
                                                [
                                                    new KeyValuePair<IColumn, ICell>(
                                                        new Column.Column(
                                                            columnName1,
                                                            new StringColumnType()
                                                        ),
                                                        new Cell(cellValue1)
                                                    ),
                                                    new KeyValuePair<IColumn, ICell>(
                                                        new Column.Column(
                                                            columnName2,
                                                            new StringColumnType()
                                                        ),
                                                        new Cell(cellValue2)
                                                    ),
                                                ],
                                                x => x.Key,
                                                x => x.Value,
                                                x => new ColumnHash(x)
                                            )
                                        ),
                                    ]
                                )
                            ),
                        ],
                        x => x.Key,
                        x => x.Value,
                        x => new TableHash(x)
                    )
                )
            ).SequenceEqual(
                new StoredSchemaDataSetHash(
                    JsonSerializer.Deserialize<IStoredSchemaDataSet>(input, _options)!
                )
            )
        );
    }

    [Fact]
    public void RoundTrip()
    {
        IString columnName1 = new RandomString(new Char('a'), new Char('z'));

        IString columnName2 = new RandomString(new Char('a'), new Char('z'));

        IString cellValue1 = new RandomString(new Char('a'), new Char('z'));

        IString cellValue2 = new RandomString(new Char('a'), new Char('z'));

        IString tableName = new RandomString(new Char('a'), new Char('z'));

        IString schemaName = new RandomString(new Char('a'), new Char('z'));

        IStoredSchemaDataSet schemaDataset = new StoredSchemaDataset(
            new Schema.Schema(
                schemaName,
                [
                    new Table.Table(
                        tableName,
                        [
                            new Column.Column(columnName1, new StringColumnType()),
                            new Column.Column(columnName2, new StringColumnType()),
                        ],
                        []
                    ),
                ],
                []
            ),
            new Collections.Generic.Dictionary<
                KeyValuePair<ITable, IStoredTableDataSet>,
                ITable,
                IStoredTableDataSet
            >(
                [
                    new KeyValuePair<ITable, IStoredTableDataSet>(
                        new Table.Table(
                            tableName,
                            [
                                new Column.Column(columnName1, new StringColumnType()),
                                new Column.Column(columnName2, new StringColumnType()),
                            ],
                            []
                        ),
                        new FakeStoredTableDataset(
                            new Table.Table(
                                tableName,
                                [
                                    new Column.Column(
                                        columnName1,
                                        new StringColumnType()
                                    ),
                                    new Column.Column(
                                        columnName2,
                                        new StringColumnType()
                                    ),
                                ],
                                []
                            ),
                            [
                                new Row(
                                    new Collections.Generic.Dictionary<
                                        KeyValuePair<IColumn, ICell>,
                                        IColumn,
                                        ICell
                                    >(
                                        [
                                            new KeyValuePair<IColumn, ICell>(
                                                new Column.Column(
                                                    columnName1,
                                                    new StringColumnType()
                                                ),
                                                new Cell(cellValue1)
                                            ),
                                            new KeyValuePair<IColumn, ICell>(
                                                new Column.Column(
                                                    columnName2,
                                                    new StringColumnType()
                                                ),
                                                new Cell(cellValue2)
                                            ),
                                        ],
                                        x => x.Key,
                                        x => x.Value,
                                        x => new ColumnHash(x)
                                    )
                                ),
                            ]
                        )
                    ),
                ],
                x => x.Key,
                x => x.Value,
                x => new TableHash(x)
            )
        );

        IStoredSchemaDataSet deserialized =
            JsonSerializer.Deserialize<IStoredSchemaDataSet>(
                JsonSerializer.Serialize(schemaDataset, _options),
                _options
            )!;

        Assert.True(
            new StoredSchemaDataSetHash(schemaDataset).SequenceEqual(
                new StoredSchemaDataSetHash(deserialized)
            )
        );
    }

    [Fact]
    public void WriteEmptyDatasets()
    {
        IString schemaName = new RandomString(new Char('a'), new Char('z'));

        IStoredSchemaDataSet schemaDataset = new StoredSchemaDataset(
            new Schema.Schema(schemaName, [], []),
            new Collections.Generic.Dictionary<
                KeyValuePair<ITable, IStoredTableDataSet>,
                ITable,
                IStoredTableDataSet
            >([], x => x.Key, x => x.Value, x => new TableHash(x))
        );

        string serialized = JsonSerializer.Serialize(schemaDataset, _options);

        Assert.Equal(
            $$"""
            {
              "Schema": {
                "Name": "{{schemaName.TextValue}}",
                "Tables": [],
                "ForeignKeys": []
              },
              "Datasets": []
            }
            """,
            serialized
        );
    }

    [Fact]
    public void ReadEmptyDatasets()
    {
        IString schemaName = new RandomString(new Char('a'), new Char('z'));

        IStoredSchemaDataSet expected = new StoredSchemaDataset(
            new Schema.Schema(schemaName, [], []),
            new Collections.Generic.Dictionary<
                KeyValuePair<ITable, IStoredTableDataSet>,
                ITable,
                IStoredTableDataSet
            >([], x => x.Key, x => x.Value, x => new TableHash(x))
        );

        string input = $$"""
            {
              "Schema": {
                "Name": "{{schemaName.TextValue}}",
                "Tables": [],
                "ForeignKeys": []
              },
              "Datasets": []
            }
            """;

        Assert.True(
            new StoredSchemaDataSetHash(expected).SequenceEqual(
                new StoredSchemaDataSetHash(
                    JsonSerializer.Deserialize<IStoredSchemaDataSet>(input, _options)!
                )
            )
        );
    }
}
