using System.Net;
using System.Text.Json;
using CustomerOrders.Api.Contracts;
using CustomerOrders.Core.Exceptions;

namespace CustomerOrders.Api.Middleware;

// Single point mapping domain exceptions to HTTP status codes — same role as
// a Spring @RestControllerAdvice. Controllers stay free of try/catch for
// these two business rules.
public sealed class ExceptionHandlingMiddleware(RequestDelegate next)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (InactiveCustomerException exception)
        {
            await WriteProblemAsync(context, HttpStatusCode.Conflict, exception.Code, exception.Message);
        }
        catch (CustomerHasOrdersException exception)
        {
            await WriteProblemAsync(context, HttpStatusCode.Conflict, exception.Code, exception.Message);
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, HttpStatusCode status, string code, string detail)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDto(code, detail), JsonOptions));
    }
}
