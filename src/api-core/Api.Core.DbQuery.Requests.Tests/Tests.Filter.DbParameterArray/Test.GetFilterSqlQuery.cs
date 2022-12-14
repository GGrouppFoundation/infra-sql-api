using System;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

partial class DbParameterArrayFilterTest
{
    [Theory]
    [MemberData(nameof(GetFilterSqlQueryTestData))]
    public static void GetFilterSqlQuery_OperatorIsInRange_ExpectCorrectSqlQuery(DbParameterArrayFilter source, string expected)
    {
        var filter = (IDbFilter)source;
        var actual = filter.GetFilterSqlQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static void GetFilterSqlQuery_OperatorIsOutOfRange_ExpectArgumentOutOfRangeException()
    {
        const int @operator = -1;
        IDbFilter filter = new DbParameterArrayFilter("Value", (DbArrayFilterOperator)@operator, new(1, 2, 3));

        var ex = Assert.Throws<ArgumentOutOfRangeException>(Test);
        Assert.Contains(@operator.ToString(), ex.Message, StringComparison.InvariantCulture);

        void Test()
            =>
            filter.GetFilterSqlQuery();
    }
}