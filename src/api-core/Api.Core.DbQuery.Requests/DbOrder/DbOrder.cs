namespace GGroupp.Infra;

public sealed record class DbOrder
{
    public DbOrder(string fieldName, DbOrderType orderType = default)
    {
        FieldName = fieldName ?? string.Empty;
        OrderType = orderType;
    }

    public string FieldName { get; }

    public DbOrderType OrderType { get; }
}