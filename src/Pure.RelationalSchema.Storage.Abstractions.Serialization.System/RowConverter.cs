using System.Text.Json;
using System.Text.Json.Serialization;
using Pure.Collections.Generic;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.HashCodes;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System;

internal sealed record ColumnAndCellJsonModel
{
    public ColumnAndCellJsonModel(KeyValuePair<IColumn, ICell> pair)
        : this(pair.Key, pair.Value) { }

    [JsonConstructor]
    public ColumnAndCellJsonModel(IColumn column, ICell cell)
    {
        Column = column;
        Cell = cell;
    }

    public IColumn Column { get; }

    public ICell Cell { get; }
}

public sealed class RowConverter : JsonConverter<IRow>
{
    public override IRow Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        IEnumerable<ColumnAndCellJsonModel> models = JsonSerializer.Deserialize<
            IEnumerable<ColumnAndCellJsonModel>
        >(ref reader, options)!;

        return new Row(
            new Dictionary<ColumnAndCellJsonModel, IColumn, ICell>(
                models,
                x => x.Column,
                x => x.Cell,
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
        JsonSerializer.Serialize(
            writer,
            value.Cells.Select(x => new ColumnAndCellJsonModel(x)),
            options
        );
    }
}
