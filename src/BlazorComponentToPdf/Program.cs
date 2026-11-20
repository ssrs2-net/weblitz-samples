// A Razor component served as a PDF, in the web application's own process: no browser, no PDF service to call.
//
//   GET /orders/1042.pdf    the order's summary (Components/OrderSummary.razor) as a PDF
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using WeblitzPdf;
using WeblitzPdf.Samples.BlazorComponentToPdf;
using WeblitzPdf.Samples.BlazorComponentToPdf.Components;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<Orders>();
builder.Services.AddScoped<HtmlRenderer>();
var app = builder.Build();

app.MapGet("/", () => Results.Text("GET /orders/1042.pdf prints an order's summary, a Razor component, to PDF with Weblitz."));

app.MapGet("/orders/{id:int}.pdf", async (int id, Orders orders, HtmlRenderer renderer) =>
{
    if (orders.Find(id) is not { } order)
        return Results.NotFound();

    // The component's HTML, rendered as Blazor renders it on a server
    var html = await renderer.Dispatcher.InvokeAsync(async () =>
    {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?> { [nameof(OrderSummary.Order)] = order });
        return (await renderer.RenderComponentAsync<OrderSummary>(parameters)).ToHtmlString();
    });

    // ... printed as Chrome prints it
    var result = await HtmlToPdf.RenderAsync(html, new HtmlPdfOptions { PrintBackground = true });
    return Results.File(result.Pdf!, "application/pdf", $"order-{id}.pdf");
});

app.Run();
