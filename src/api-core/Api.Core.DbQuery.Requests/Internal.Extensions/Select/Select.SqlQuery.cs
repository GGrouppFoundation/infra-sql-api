using System;
using System.Text;

namespace GGroupp.Infra;

partial class DbQueryExtensions
{
    internal static string BuildSqlQuery(this DbSelectQuery query)
    {
        var queryBuilder = new StringBuilder("SELECT ");

        if (query.Top is not null && query.Offset is null)
        {
            queryBuilder = queryBuilder.Append("TOP ").Append(query.Top.Value).Append(' ');
        }

        if (query.SelectedFields.IsEmpty)
        {
            queryBuilder = queryBuilder.Append('*');
        }
        else
        {
            queryBuilder = queryBuilder.AppendJoined(query.SelectedFields.AsEnumerable());
        }

        queryBuilder = queryBuilder.Append(" FROM ").Append(query.TableName);
        if (string.IsNullOrWhiteSpace(query.ShortName) is false)
        {
            queryBuilder = queryBuilder.Append(' ').Append(query.ShortName);
        }

        var joinQuery = query.JoinedTables.BuildSqlQuery();
        if (string.IsNullOrWhiteSpace(joinQuery) is false)
        {
            queryBuilder = queryBuilder.Append(' ').Append(joinQuery);
        }

        var filterQuery = query.Filter?.GetFilterSqlQuery();
        if (string.IsNullOrWhiteSpace(filterQuery) is false)
        {
            queryBuilder = queryBuilder.Append(" WHERE ").Append(filterQuery);
        }

        var orderByQuery = query.Orders.BuildSqlQuery();
        if (string.IsNullOrWhiteSpace(orderByQuery) is false)
        {
            queryBuilder = queryBuilder.Append(" ORDER BY ").Append(orderByQuery);
        }

        if (query.Offset is null)
        {
            return queryBuilder.ToString();
        }

        queryBuilder = queryBuilder.Append(" OFFSET ").Append(query.Offset.Value);
        if (query.Top is null)
        {
            return queryBuilder.ToString();
        }

        return queryBuilder.Append(" ROWS FETCH NEXT ").Append(query.Top.Value).Append(" ROWS ONLY").ToString();
    }

    private static string BuildSqlQuery(this FlatArray<DbJoinedTable> joinedTables)
    {
        if (joinedTables.IsEmpty)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        foreach (var joinedTable in joinedTables)
        {
            if (builder.Length > 0)
            {
                builder = builder.Append(' ');
            }

            builder = builder.AppendJoinedTable(joinedTable);
        }

        return builder.ToString();
    }

    private static StringBuilder AppendJoinedTable(this StringBuilder builder, DbJoinedTable joinedTable)
        =>
        builder.Append(
            joinedTable.Type.GetName())
        .Append(
            " JOIN ")
        .Append(
            joinedTable.TableName)
        .Append(
            ' ')
        .Append(
            joinedTable.ShortName)
        .Append(
            " ON ")
        .Append(
            joinedTable.Filter.GetFilterSqlQuery());
}