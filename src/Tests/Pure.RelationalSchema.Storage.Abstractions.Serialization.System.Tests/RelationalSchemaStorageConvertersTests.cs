using System.Collections;
using System.Text.Json.Serialization;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System.Tests;

public sealed record RelationalSchemaStorageConvertersTests
{
    [Fact]
    public void EnumeratesFourConverters()
    {
        Assert.Equal(4, new RelationalSchemaStorageConverters().Count());
    }

    [Fact]
    public void EnumeratesCorrectConverterTypes()
    {
        IEnumerable<JsonConverter> converters = new RelationalSchemaStorageConverters();

        _ = Assert.IsType<CellConverter>(converters.ElementAt(0));
        _ = Assert.IsType<RowConverter>(converters.ElementAt(1));
        _ = Assert.IsType<StoredTableDataSetConverter>(converters.ElementAt(2));
        _ = Assert.IsType<StoredSchemaDataSetConverter>(converters.ElementAt(3));
    }

    [Fact]
    public void NonGenericEnumeratorReturnsAllConverters()
    {
        IEnumerable converters = new RelationalSchemaStorageConverters();

        int count = 0;

        foreach (object _ in converters)
        {
            count++;
        }

        Assert.Equal(4, count);
    }
}
