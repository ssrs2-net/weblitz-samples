# Weblitz samples

Samples for [Weblitz](https://weblitzpdf.com), the [WeblitzPdf](https://www.nuget.org/packages/WeblitzPdf) package:
HTML, CSS and JavaScript printed to PDF as Chrome prints them, in your .NET process, with no browser to install or
start. Each sample references the package from NuGet.org, as your application would.

| Sample | What it shows |
|---|---|
| [src/Templates](src/Templates) | An invoice, a receipt, a bank statement, a certificate and a shipping label. Each is a Razor component, rendered to HTML by ASP.NET Core's `HtmlRenderer` and printed to PDF on the paper its own `@page` rule names (A4, an 80 mm receipt roll, A4 landscape, a 4 by 6 inch label). They use page numbers in margin boxes, a table header repeated on each page and a Code 39 barcode drawn as SVG. |
| [src/BlazorComponentToPdf](src/BlazorComponentToPdf) | A web application serving a Razor component as a PDF: `GET /orders/1042.pdf`. |
| [src/PuppeteerMigration](src/PuppeteerMigration) | The same invoice printed with PuppeteerSharp (`Before`) and with Weblitz (`After`), with a Dockerfile each: the options keep Puppeteer's names, and the browser leaves the image. |
| [src/OfficeExport](src/OfficeExport) | One page as a PDF, an Excel workbook whose figures are numbers, a Word document and PowerPoint slides. No Office on the machine. |
| [tests](tests) | Every sample run and its output read back. |

## Run them

You need the .NET 10 SDK.

```bash
dotnet run --project src/Templates            # out/invoice.pdf, receipt.pdf, bank-statement.pdf, certificate.pdf, shipping-label.pdf
dotnet run --project src/OfficeExport         # out/report.pdf, .xlsx, .docx, .pptx
dotnet run --project src/BlazorComponentToPdf # then open http://localhost:5000/orders/1042.pdf
dotnet run --project src/PuppeteerMigration/After
dotnet test tests/WeblitzPdf.Samples.Tests
```

Without a licence key, every page carries Weblitz's trial watermark. A key (the `SSRS2_LICENSE` environment variable,
or `License.Activate`) takes it off.

The package carries its script engine for every platform. The samples build for your machine's platform alone
(`Directory.Build.props`); publish your application for the platform it runs on, for example
`dotnet publish -r linux-x64`. On Alpine, add the `gcompat` package to the image and build with
`-r linux-musl-x64`.

## From Puppeteer

PuppeteerSharp:

```csharp
await new BrowserFetcher().DownloadAsync();
await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
await using var page = await browser.NewPageAsync();
await page.SetContentAsync(html);
var pdf = await page.PdfDataAsync(new PdfOptions
{
    Format = PaperFormat.A4,
    PrintBackground = true,
    MarginOptions = new MarginOptions { Top = "20mm", Bottom = "20mm" },
});
```

Weblitz:

```csharp
var pdf = HtmlToPdf.Render(html, new HtmlPdfOptions
{
    Format = "A4",
    PrintBackground = true,
    Margin = new HtmlMargins(Top: "20mm", Bottom: "20mm"),
}).Pdf;
```

The two images differ by the browser:

```bash
docker build -f src/PuppeteerMigration/Before/Dockerfile -t invoice-before .
docker build -f src/PuppeteerMigration/After/Dockerfile -t invoice-after .
docker images invoice-before invoice-after
```

## More

- [Getting started](https://weblitzpdf.com/docs)
- [The measurements](https://weblitzpdf.com/proof): every document beside Chrome's PDF, and the other libraries
- [Weblitz beside the other libraries](https://weblitzpdf.com/compare)
- [A showcase API: a web address in, its PDF out](https://github.com/ssrs2-net/weblitz-url-to-pdf)

The samples are MIT-licensed (LICENSE). Weblitz itself is licensed by [SSRS2](https://ssrs2.net/pricing).
