namespace WeblitzPdf.Samples.PuppeteerMigration.After;

/// <summary>After: Puppeteer's page.pdf() options by the same names, and no browser to download, start or keep alive.</summary>
public static class Invoices
{
    public static async Task<byte[]> PdfAsync(string html)
    {
        var result = await HtmlToPdf.RenderAsync(html, new HtmlPdfOptions
        {
            Format = "A4",
            PrintBackground = true,
            Margin = new HtmlMargins(Top: "20mm", Right: "16mm", Bottom: "20mm", Left: "16mm"),
        });
        return result.Pdf!;
    }
}
