using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TrackEasy.Shared.Infrastructure;

public sealed class RazorRenderer(HtmlRenderer htmlRenderer)
{
    public async Task<string> RenderHtmlToString<TComponent>() where TComponent : IComponent
    {
        var html = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var output = await htmlRenderer.RenderComponentAsync<TComponent>();

            return output.ToHtmlString();
        });

        return html;
    }
    
    public async Task<string> RenderHtmlToString<TComponent>(Dictionary<string, object?> componentParameters) where TComponent : IComponent
    {
        var html = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var parameters = ParameterView.FromDictionary(componentParameters);
            var output = await htmlRenderer.RenderComponentAsync<TComponent>(parameters);

            return output.ToHtmlString();
        });

        return html;
    }
}