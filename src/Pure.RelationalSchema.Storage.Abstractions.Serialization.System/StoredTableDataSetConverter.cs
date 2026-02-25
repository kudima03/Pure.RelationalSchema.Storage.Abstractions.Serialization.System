using System.Collections;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Pure.RelationalSchema.Abstractions.Table;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System;

internal sealed record StoredTableDataSetJsonModel : IStoredTableDataSet
{
    public StoredTableDataSetJsonModel(IStoredTableDataSet dataset)
        : this(dataset.TableSchema, dataset) { }

    [JsonConstructor]
    public StoredTableDataSetJsonModel(ITable tableSchema, IEnumerable<IRow> rows)
        : this(tableSchema, rows.AsQueryable()) { }

    public StoredTableDataSetJsonModel(ITable tableSchema, IQueryable<IRow> rows)
    {
        TableSchema = tableSchema;
        Rows = rows;
    }

    public ITable TableSchema { get; }

    public IQueryable<IRow> Rows { get; }

    [JsonIgnore]
    public Type ElementType => Rows.ElementType;

    [JsonIgnore]
    public Expression Expression => Rows.Expression;

    [JsonIgnore]
    public IQueryProvider Provider => Rows.Provider;

    public async IAsyncEnumerator<IRow> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        foreach (IRow row in Rows)
        {
            yield return await Task.FromResult(row);
        }
    }

    public IEnumerator<IRow> GetEnumerator()
    {
        return Rows.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public sealed class StoredTableDataSetConverter : JsonConverter<IStoredTableDataSet>
{
    public override IStoredTableDataSet Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        return JsonSerializer.Deserialize<StoredTableDataSetJsonModel>(
            ref reader,
            options
        )!;
    }

    public override void Write(
        Utf8JsonWriter writer,
        IStoredTableDataSet value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, new StoredTableDataSetJsonModel(value), options);
    }
}
