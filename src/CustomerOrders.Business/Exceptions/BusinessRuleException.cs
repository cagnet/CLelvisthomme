namespace CustomerOrders.Business.Exceptions;

public sealed class BusinessRuleException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}
