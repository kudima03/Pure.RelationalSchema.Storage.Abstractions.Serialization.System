using System.Text.Json;
using System.Text.Json.Serialization;
using Pure.Primitives.Abstractions.String;
using Pure.Primitives.Random.String;
using Pure.RelationalSchema.Abstractions.Column;
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
        IEnumerable<IString> strings =
        [
            .. Enumerable
                .Range(0, 10)
                .Select(_ => new RandomString(new Char('a'), new Char('z'))),
        ];

        IEnumerable<ICell> cells = strings.Select(x => new Cell(x));

        IRow row = new Row(
            new Collections.Generic.Dictionary<ICell, IColumn, ICell>(
                cells,
                x => new Column.Column(new RandomString(), new StringColumnType()),
                x => x,
                x => new ColumnHash(x)
            )
        );

        string serialized = JsonSerializer.Serialize(row, _options);

        Assert.Equal(
            $$"""
            {
              "Value": ""
            }
            """,
            serialized
        );
    }

    [Fact]
    public void Read()
    {
        IString value = new RandomString(new Char('a'), new Char('z'));

        string input = $$"""
            {
              "Value": "{{value.TextValue}}"
            }
            """;

        Assert.True(
            new CellHash(new Cell(value)).SequenceEqual(
                new CellHash(JsonSerializer.Deserialize<ICell>(input, _options)!)
            )
        );
    }
}
