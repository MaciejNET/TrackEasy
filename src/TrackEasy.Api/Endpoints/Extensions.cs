namespace TrackEasy.Api.Endpoints;

public static class Extensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapDiscountCodesEndpoints();
        return app;
    }
}