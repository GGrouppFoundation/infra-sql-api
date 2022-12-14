using System;

namespace GGroupp.Infra;

public sealed record class DbSelectQuery : IDbQuery
{
    public DbSelectQuery(string tableName)
        =>
        TableName = tableName ?? string.Empty;

    public DbSelectQuery(string tableName, string shortName)
    {
        TableName = tableName ?? string.Empty;
        ShortName = string.IsNullOrWhiteSpace(shortName) ? null : shortName;
    }

    public string TableName { get; }

    public string? ShortName { get; }

    public int? Top { get; init; }

    public long? Offset { get; init; }

    public FlatArray<string> SelectedFields { get; init; }

    public IDbFilter? Filter { get; init; }

    public FlatArray<DbJoinedTable> JoinedTables { get; init; }

    public FlatArray<DbOrder> Orders { get; init; }

    string IDbQuery.GetSqlQuery()
        =>
        this.BuildSqlQuery();

    FlatArray<DbParameter> IDbQuery.GetParameters()
        =>
        this.BuildParameters();
}