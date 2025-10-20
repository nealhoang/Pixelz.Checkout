namespace Pixelz.Api.Extensions;

public static class EndpointMappingExtensions
{
    public static void MapPixelzEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapOrderEndpoints();
    }
}
