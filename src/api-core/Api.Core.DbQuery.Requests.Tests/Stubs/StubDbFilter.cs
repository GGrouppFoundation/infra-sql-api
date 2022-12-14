using System;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

internal sealed partial class StubDbFilter : IDbFilter
{
    private readonly string sqlQuery;

    private readonly FlatArray<DbParameter> parameters;

    public StubDbFilter(string sqlQuery, params DbParameter[] parameters)
    {
        this.sqlQuery = sqlQuery;
        this.parameters = parameters;
    }

    public string GetFilterSqlQuery()
        =>
        sqlQuery;

    public FlatArray<DbParameter> GetFilterParameters()
        =>
        parameters;
}