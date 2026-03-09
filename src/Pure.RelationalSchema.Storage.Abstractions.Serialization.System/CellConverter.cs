using System.Text.Json;
using System.Text.Json.Serialization;
using String = Pure.Primitives.String.String;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System;

internal sealed record CellJsonModel
{
    public CellJsonModel(ICell cell)
        : this(cell.Value.TextValue) { }

    [JsonConstructor]
    public CellJsonModel(string value)
    {
        Value = value;
    }

    public string Value { get; }
}

public sealed class CellConverter : JsonConverter<ICell>
{
    public override ICell Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        CellJsonModel model = JsonSerializer.Deserialize<CellJsonModel>(
            ref reader,
            options
        )!;
        return new Cell(new String(model.Value));
    }

    public override void Write(
        Utf8JsonWriter writer,
        ICell value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new CellJsonModel(value), options);
    }
}
