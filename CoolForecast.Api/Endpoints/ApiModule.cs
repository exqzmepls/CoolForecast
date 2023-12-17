using Carter;

namespace CoolForecast.Api.Endpoints;

public abstract class ApiModule(string prefix, string tag) : CarterModule("api")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(prefix).WithTags(tag);
        AddEndpoints(group);
    }

    protected abstract void AddEndpoints(RouteGroupBuilder group);
}