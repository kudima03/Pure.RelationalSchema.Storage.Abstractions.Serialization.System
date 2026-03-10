using System.Collections;
using System.Linq.Expressions;
using Pure.RelationalSchema.Abstractions.Table;

namespace Pure.RelationalSchema.Storage.Abstractions.Serialization.System.Tests.Fakes;

internal sealed record FakeStoredTableDataset : IStoredTableDataSet
{
    private readonly IQueryable<IRow> _rows;

    public FakeStoredTableDataset(ITable tableSchema, IEnumerable<IRow> rows)
        : this(tableSchema, rows.AsQueryable()) { }

    public FakeStoredTableDataset(ITable tableSchema, IQueryable<IRow> rows)
    {
        TableSchema = tableSchema;
        _rows = rows;
    }

    public ITable TableSchema { get; }

    public Type ElementType => _rows.ElementType;

    public Expression Expression => _rows.Expression;

    public IQueryProvider Provider => _rows.Provider;

    public IAsyncEnumerator<IRow> GetAsyncEnumerator(
        CancellationToken cancellationToken = default
    )
    {
        return _rows.ToAsyncEnumerable().GetAsyncEnumerator(cancellationToken);
    }

    public IEnumerator<IRow> GetEnumerator()
    {
        return _rows.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
