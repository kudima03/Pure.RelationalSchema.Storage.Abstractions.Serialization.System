using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using Pure.Collections.Generic;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.HashCodes;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System;

internal sealed record CellsJsonModel : IEnumerable<KeyValuePair<string, string>>
{
    private readonly IEnumerable<KeyValuePair<string, string>> _cells;

    public CellsJsonModel(IEnumerable<ICell> cells) : this(cells.Select(x=> new Key))
    {

    }

    [JsonConstructor]
    public CellsJsonModel(IEnumerable<KeyValuePair<string, string>> cells)
    {
        _cells = cells;
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return _cells.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

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
        JsonSerializer.Serialize(writer, value.Cells.Select(x=> new KeyValuePair<string, string>(x.Key.)).AsEnumerable(), options);
    }
}
