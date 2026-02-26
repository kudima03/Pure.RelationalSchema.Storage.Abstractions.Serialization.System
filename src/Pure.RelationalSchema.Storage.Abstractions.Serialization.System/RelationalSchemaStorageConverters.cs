using System.Collections;
using System.Text.Json.Serialization;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System;

public sealed class RelationalSchemaStorageConverters : IEnumerable<JsonConverter>
{
    public IEnumerator<JsonConverter> GetEnumerator()
    {
        yield return new CellConverter();
        yield return new RowConverter();
        yield return new StoredTableDataSetConverter();
        yield return new StoredSchemaDataSetConverter();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
