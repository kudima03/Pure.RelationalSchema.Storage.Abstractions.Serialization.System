using System.Text.Json;
using System.Text.Json.Serialization;
using Pure.Primitives.Abstractions.String;
using Pure.Primitives.Random.String;
using Pure.RelationalSchema.Storage.HashCodes;
using Char = Pure.Primitives.Char.Char;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System.Tests;

public sealed record CellConverterTests
{
    private readonly JsonSerializerOptions _options;

    public CellConverterTests()
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
        IString value = new RandomString(new Char('a'), new Char('z'));

        ICell cell = new Cell(value);

        string serialized = JsonSerializer.Serialize(cell, _options);

        Assert.Equal(
            $$"""
            {
              "Value": "{{value.TextValue}}"
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

    [Fact]
    public void RoundTrip()
    {
        IString value = new RandomString(new Char('a'), new Char('z'));

        ICell cell = new Cell(value);

        ICell deserialized = JsonSerializer.Deserialize<ICell>(
            JsonSerializer.Serialize(cell, _options),
            _options
        )!;

        Assert.True(new CellHash(cell).SequenceEqual(new CellHash(deserialized)));
    }
}
