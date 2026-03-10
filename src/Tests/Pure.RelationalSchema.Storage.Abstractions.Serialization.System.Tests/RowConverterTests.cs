using System.Text.Json;
using System.Text.Json.Serialization;
using Pure.Primitives.Abstractions.Serialization.System;
using Pure.Primitives.Abstractions.String;
using Pure.Primitives.Random.String;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.Abstractions.Serialization.System;
using Pure.RelationalSchema.ColumnType;
using Pure.RelationalSchema.HashCodes;
using Pure.RelationalSchema.Storage.HashCodes;
using Char = Pure.Primitives.Char.Char;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System.Tests;

public sealed record RowConverterTests
{
    private readonly JsonSerializerOptions _options;

    public RowConverterTests()
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

        IString value1 = new RandomString(new Char('a'), new Char('z'));

        IString value2 = new RandomString(new Char('a'), new Char('z'));

        IRow row = new Row(
            new Collections.Generic.Dictionary<
                KeyValuePair<IColumn, ICell>,
                IColumn,
                ICell
            >(
                [
                    new KeyValuePair<IColumn, ICell>(
                        new Column.Column(columnName1, new StringColumnType()),
                        new Cell(value1)
                    ),
                    new KeyValuePair<IColumn, ICell>(
                        new Column.Column(columnName2, new StringColumnType()),
                        new Cell(value2)
                    ),
                ],
                x => x.Key,
                x => x.Value,
                x => new ColumnHash(x)
            )
        );

        string serialized = JsonSerializer.Serialize(row, _options);

        Assert.Equal(
            $$"""
            [
              {
                "Column": {
                  "Name": "{{columnName1.TextValue}}",
                  "Type": "string"
                },
                "Cell": {
                  "Value": "{{value1.TextValue}}"
                }
              },
              {
                "Column": {
                  "Name": "{{columnName2.TextValue}}",
                  "Type": "string"
                },
                "Cell": {
                  "Value": "{{value2.TextValue}}"
                }
              }
            ]
            """,
            serialized
        );
    }

    [Fact]
    public void Read()
    {
        IString columnName1 = new RandomString(new Char('a'), new Char('z'));

        IString columnName2 = new RandomString(new Char('a'), new Char('z'));

        IString value1 = new RandomString(new Char('a'), new Char('z'));

        IString value2 = new RandomString(new Char('a'), new Char('z'));

        string input = $$"""
            [
              {
                "Column": {
                  "Name": "{{columnName1.TextValue}}",
                  "Type": "string"
                },
                "Cell": {
                  "Value": "{{value1.TextValue}}"
                }
              },
              {
                "Column": {
                  "Name": "{{columnName2.TextValue}}",
                  "Type": "string"
                },
                "Cell": {
                  "Value": "{{value2.TextValue}}"
                }
              }
            ]
            """;

        Assert.True(
            new RowHash(
                new Row(
                    new Collections.Generic.Dictionary<
                        KeyValuePair<IColumn, ICell>,
                        IColumn,
                        ICell
                    >(
                        [
                            new KeyValuePair<IColumn, ICell>(
                                new Column.Column(columnName1, new StringColumnType()),
                                new Cell(value1)
                            ),
                            new KeyValuePair<IColumn, ICell>(
                                new Column.Column(columnName2, new StringColumnType()),
                                new Cell(value2)
                            ),
                        ],
                        x => x.Key,
                        x => x.Value,
                        x => new ColumnHash(x)
                    )
                )
            ).SequenceEqual(
                new RowHash(JsonSerializer.Deserialize<IRow>(input, _options)!)
            )
        );
    }
}
