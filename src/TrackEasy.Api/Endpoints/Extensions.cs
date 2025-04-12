namespace TrackEasy.Api.Endpoints;

public static class Extensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapDiscountCodesEndpoints();
        app.MapDiscountsEndpoints();
        app.MapStationsEndpoints();
        return app;
    }
}