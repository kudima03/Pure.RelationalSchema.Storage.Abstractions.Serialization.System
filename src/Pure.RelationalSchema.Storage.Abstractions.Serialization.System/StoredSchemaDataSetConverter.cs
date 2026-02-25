using System.Text.Json;
using System.Text.Json.Serialization;
using Pure.Collections.Generic;
using Pure.RelationalSchema.Abstractions.Schema;
using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.HashCodes;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System;

internal sealed record StoredSchemaDatasetJsonModel
{
    public StoredSchemaDatasetJsonModel(IStoredSchemaDataSet dataset)
        : this(dataset.Schema, dataset.Select(x => x.Value)) { }

    [JsonConstructor]
    public StoredSchemaDatasetJsonModel(
        ISchema schema,
        IEnumerable<IStoredTableDataSet> tables
    )
    {
        Schema = schema;
        Tables = tables;
    }

    public ISchema Schema { get; }

    public IEnumerable<IStoredTableDataSet> Tables { get; }
}

public sealed class StoredSchemaDataSetConverter : JsonConverter<IStoredSchemaDataSet>
{
    public override IStoredSchemaDataSet Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        StoredSchemaDatasetJsonModel jsonModel =
            JsonSerializer.Deserialize<StoredSchemaDatasetJsonModel>(
                ref reader,
                options
            )!;

        return new StoredSchemaDataset(
            jsonModel.Schema,
            new Dictionary<IStoredTableDataSet, ITable, IStoredTableDataSet>(
                jsonModel.Tables,
                x => x.TableSchema,
                x => x,
                x => new TableHash(x)
            )
        );
    }

    public override void Write(
        Utf8JsonWriter writer,
        IStoredSchemaDataSet value,
        JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(
            writer,
            new StoredSchemaDatasetJsonModel(value),
            options
        );
    }
}
