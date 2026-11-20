using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WeblitzPdf.Samples.Templates;

/// <summary>A Razor component as an HTML document (ASP.NET Core's HtmlRenderer, no web server), then as a PDF.</summary>
public static class DocumentRenderer
{
    private static readonly ServiceProvider Services = new ServiceCollection().AddLogging().BuildServiceProvider();

    /// <summary>How the documents print: their own @page rules give the paper and margins (a receipt's roll, a label's
    /// 4 by 6 inches), backgrounds included.</summary>
    public static readonly HtmlPdfOptions Options = new() { PreferCssPageSize = true, PrintBackground = true };

    /// <summary>The component's HTML, its parameters given by name.</summary>
    public static async Task<string> HtmlAsync<TComponent>(IDictionary<string, object?> parameters) where TComponent : IComponent
    {
        await using var renderer = new HtmlRenderer(Services, Services.GetRequiredService<ILoggerFactory>());
        return await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var output = await renderer.RenderComponentAsync<TComponent>(ParameterView.FromDictionary(parameters));
            return output.ToHtmlString();
        });
    }

    /// <summary>The component printed to PDF.</summary>
    public static async Task<HtmlPdfResult> PdfAsync<TComponent>(IDictionary<string, object?> parameters) where TComponent : IComponent =>
        await HtmlToPdf.RenderAsync(await HtmlAsync<TComponent>(parameters), Options);
}
