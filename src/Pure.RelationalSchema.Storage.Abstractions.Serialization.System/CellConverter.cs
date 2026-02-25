using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System;

public sealed class CellConverter : JsonConverter<ICell>
{
    public override ICell Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        return JsonSerializer.Deserialize<Cell>(ref reader, options)!;
    }

    public override void Write(
        Utf8JsonWriter writer,
        ICell value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
