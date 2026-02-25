using System.Text.Json;
using System.Text.Json.Serialization;
using Pure.Collections.Generic;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.HashCodes;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System;

public sealed class RowConverter : JsonConverter<IRow>
{
    public override IRow Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        IEnumerable<KeyValuePair<IColumn, ICell>> pairs = JsonSerializer.Deserialize<
            IEnumerable<KeyValuePair<IColumn, ICell>>
        >(ref reader, options)!;

        return new Row(
            new Dictionary<KeyValuePair<IColumn, ICell>, IColumn, ICell>(
                pairs,
                x => x.Key,
                x => x.Value,
                x => new ColumnHash(x)
            )
        );
    }

    public override void Write(
        Utf8JsonWriter writer,
        IRow value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value.Cells.AsEnumerable(), options);
    }
}
