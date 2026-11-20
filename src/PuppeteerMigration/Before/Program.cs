// Before: HTML to PDF with PuppeteerSharp. A browser is downloaded (or installed in the image), started, given a tab and the
// page, then asked for the PDF; the application keeps it alive, restarts it when it hangs and patches it.
using PuppeteerSharp;
using PuppeteerSharp.Media;

var html = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "invoice.html"));

var installed = Environment.GetEnvironmentVariable("PUPPETEER_EXECUTABLE_PATH");
if (installed is null)
    await new BrowserFetcher().DownloadAsync();
await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
{
    Headless = true,
    ExecutablePath = installed,
    Args = ["--no-sandbox"], // as root in a container
});
await using var page = await browser.NewPageAsync();
await page.SetContentAsync(html);
var pdf = await page.PdfDataAsync(new PdfOptions
{
    Format = PaperFormat.A4,
    PrintBackground = true,
    MarginOptions = new MarginOptions { Top = "20mm", Right = "16mm", Bottom = "20mm", Left = "16mm" },
});
await File.WriteAllBytesAsync("invoice.pdf", pdf);
Console.WriteLine($"invoice.pdf: {pdf.Length / 1024} KB");
