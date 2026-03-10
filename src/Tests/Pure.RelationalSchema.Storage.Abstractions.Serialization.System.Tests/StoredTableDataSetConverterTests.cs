using System.Text.Json;
using System.Text.Json.Serialization;
using Pure.Primitives.Abstractions.Serialization.System;
using Pure.Primitives.Abstractions.String;
using Pure.Primitives.Random.String;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.Abstractions.Serialization.System;
using Pure.RelationalSchema.ColumnType;
using Pure.RelationalSchema.HashCodes;
using Pure.RelationalSchema.Storage.Abstractions.Serialization.System.Tests.Fakes;
using Pure.RelationalSchema.Storage.HashCodes;
using Char = Pure.Primitives.Char.Char;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System.Tests;

public sealed record StoredTableDataSetConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StoredTableDataSetConverterTests()
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

        IStoredTableDataSet dataset = new FakeStoredTableDataset(
            new Table.Table(
                tableName,
                [
                    new Column.Column(columnName1, new StringColumnType()),
                    new Column.Column(columnName2, new StringColumnType()),
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
                                new Column.Column(columnName1, new StringColumnType()),
                                new Cell(cellValue1)
                            ),
                            new KeyValuePair<IColumn, ICell>(
                                new Column.Column(columnName2, new StringColumnType()),
                                new Cell(cellValue2)
                            ),
                        ],
                        x => x.Key,
                        x => x.Value,
                        x => new ColumnHash(x)
                    )
                ),
            ]
        );

        string serialized = JsonSerializer.Serialize(dataset, _options);

        Assert.Equal(
            $$"""
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

        IStoredTableDataSet dataset = new FakeStoredTableDataset(
            new Table.Table(
                tableName,
                [
                    new Column.Column(columnName1, new StringColumnType()),
                    new Column.Column(columnName2, new StringColumnType()),
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
                                new Column.Column(columnName1, new StringColumnType()),
                                new Cell(cellValue1)
                            ),
                            new KeyValuePair<IColumn, ICell>(
                                new Column.Column(columnName2, new StringColumnType()),
                                new Cell(cellValue2)
                            ),
                        ],
                        x => x.Key,
                        x => x.Value,
                        x => new ColumnHash(x)
                    )
                ),
            ]
        );

        string input = $$"""
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
            """;

        Assert.True(
            new StoredTableDataSetHash(dataset).SequenceEqual(
                new StoredTableDataSetHash(
                    JsonSerializer.Deserialize<IStoredTableDataSet>(input, _options)!
                )
            )
        );
    }
}
