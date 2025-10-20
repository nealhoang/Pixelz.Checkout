using System.ComponentModel;

namespace Pixelz.Api.Endpoints.Orders;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/orders")
                       .WithTags("Orders");

        group.MapGet("", async (
            ISender sender,
            [FromQuery(Name = "keyword"), Description("Optional search term (order name).")] string? keyword,
            [FromQuery(Name = "page"), Description("Page number (1-based index). Default = 1.")] int page = 1,
            [FromQuery(Name = "size"), Description("Page size (number of records per page). Default = 15.")] int size = 15,
            CancellationToken ct = default) =>
        {
            if (page <= 0)
            {
                page = 1;
            }

            if (size <= 0)
            {
                size = 15;
            }

            var query = new SearchOrdersQuery(keyword, page, size);

            PagedResult<OrderDto> result = await sender.Send(query, ct);
            return Results.Ok(result);
        })
        .WithName("SearchOrders")
        .Produces<PagedResult<Order>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Search and paginate orders")
        .WithDescription("Retrieves a paginated list of orders optionally filtered by keyword.");

        group.MapPost("{id:long}/checkout", async (ISender sender, long id, CancellationToken ct = default) =>
        {
            var cmd = new CheckoutOrderCommand(id);
            var success = await sender.Send(cmd, ct);
            return success
                ? Results.NoContent()
                : Results.BadRequest(new { message = "Payment failed." });
        })
        .WithName("CheckoutOrder")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Checkout an order and process payment")
        .WithDescription("Processes payment for a given order, updates its status, and publishes related integration events (Email, Invoice, Production).");

        return app;
    }
}
