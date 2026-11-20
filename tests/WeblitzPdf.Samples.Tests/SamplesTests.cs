using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;
using UglyToad.PdfPig;
using WeblitzPdf.Samples.OfficeExport;
using WeblitzPdf.Samples.PuppeteerMigration.After;
using WeblitzPdf.Samples.Templates;
using Xunit;

namespace WeblitzPdf.Samples.Tests;

/// <summary>What each sample prints, read back: the text (spaces aside: a PDF places words, it need not write their
/// spaces) and the paper.</summary>
public class SamplesTests
{
    private static string Squash(string text) => Regex.Replace(text, @"\s+", "");

    private static (string Text, IReadOnlyList<(double Width, double Height)> Pages) Read(byte[] pdf)
    {
        using var document = PdfDocument.Open(pdf);
        var pages = document.GetPages().ToList();
        return (string.Join(" ", pages.Select(p => p.Text)), pages.Select(p => (p.Width, p.Height)).ToList());
    }

    [Theory]
    [InlineData("invoice", "Northwind Studio Ltd", "Total CHF")]
    [InlineData("receipt", "Café Lumière", "TOTAL CHF")]
    [InlineData("bank-statement", "Banque du Léman", "Closing balance")]
    [InlineData("certificate", "Ada Marchetti", "Certificate of completion")]
    [InlineData("shipping-label", "SP4417203GB", "EXPRESS 24")]
    public async Task EachTemplate_PrintsItsDocument(string name, string first, string second)
    {
        var result = await HtmlToPdf.RenderAsync(await Documents.HtmlAsync(name), DocumentRenderer.Options);

        Assert.True(result.Completed, name);
        var (text, _) = Read(result.Pdf!);
        // (case aside too: a label set in capitals by CSS is written in capitals)
        Assert.Contains(Squash(first), Squash(text), StringComparison.OrdinalIgnoreCase);
        Assert.Contains(Squash(second), Squash(text), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Each document's own @page size is its paper (PreferCssPageSize).</summary>
    [Theory]
    [InlineData("invoice", 595.3, 841.9)]
    [InlineData("receipt", 226.8, 481.9)]
    [InlineData("certificate", 841.9, 595.3)]
    [InlineData("shipping-label", 288, 432)]
    public async Task EachTemplate_IsOnItsOwnPaper(string name, double width, double height)
    {
        var result = await HtmlToPdf.RenderAsync(await Documents.HtmlAsync(name), DocumentRenderer.Options);

        var (_, pages) = Read(result.Pdf!);
        Assert.InRange(pages[0].Width, width - 1, width + 1);
        Assert.InRange(pages[0].Height, height - 1, height + 1);
    }

    /// <summary>The statement's month of transactions runs over pages, each page under the table's header row again.</summary>
    [Fact]
    public async Task TheStatement_RepeatsItsHeaderOnEachPage()
    {
        var result = await HtmlToPdf.RenderAsync(await Documents.HtmlAsync("bank-statement"), DocumentRenderer.Options);

        using var document = PdfDocument.Open(result.Pdf!);
        Assert.True(document.NumberOfPages >= 2, $"{document.NumberOfPages} page(s)");
        foreach (var page in document.GetPages())
            Assert.Contains("Balance", page.Text);
        Assert.Contains($"Page2of{document.NumberOfPages}", Squash(document.GetPage(2).Text));
    }

    [Fact]
    public void Code39_HasNineElementsAChar_ThreeOfThemWide_AndRefusesWhatItCannotDraw()
    {
        Assert.Equal(44, Code39.Table.Count);
        foreach (var (symbol, pattern) in Code39.Table)
        {
            Assert.Equal(9, pattern.Length);
            Assert.Equal(3, pattern.Count(c => c == '1'));
        }
        Assert.Equal(44, Code39.Table.Values.Distinct().Count());
        Assert.Contains("<svg", Code39.Svg("sp4417203gb"));
        Assert.Throws<ArgumentException>(() => Code39.Svg("a@b"));
    }

    [Fact]
    public async Task ABlazorComponent_IsServedAsAPdf()
    {
        // (any type of the sample's assembly names its entry point: every sample's Program is public, so Orders does)
        using var app = new WebApplicationFactory<WeblitzPdf.Samples.BlazorComponentToPdf.Orders>();
        var client = app.CreateClient();

        var response = await client.GetAsync("/orders/1042.pdf");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/pdf", response.Content.Headers.ContentType!.MediaType);
        var (text, _) = Read(await response.Content.ReadAsByteArrayAsync());
        Assert.Contains("Order1042", Squash(text));
        Assert.Contains("Handheldscanner", Squash(text));

        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/orders/7.pdf")).StatusCode);
    }

    /// <summary>One page as four files: the PDF, a workbook whose figures are numbers, a Word document, a deck.</summary>
    [Fact]
    public async Task OnePage_IsAPdfAWorkbookADocumentAndADeck()
    {
        var html = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "report.html"));
        var files = await Export.AllAsync(html);

        Assert.Contains("Quarterlysales", Squash(Read(files.Pdf).Text));
        using (var workbook = new ZipArchive(new MemoryStream(files.Workbook)))
        {
            Assert.NotNull(workbook.GetEntry("xl/workbook.xml"));
            var sheets = workbook.Entries.Where(e => e.FullName.StartsWith("xl/worksheets/", StringComparison.Ordinal)).Select(e => new StreamReader(e.Open()).ReadToEnd());
            Assert.Contains(sheets, sheet => sheet.Contains("<v>455300</v>"));
        }
        using (var document = new ZipArchive(new MemoryStream(files.Document)))
        {
            var body = new StreamReader(document.GetEntry("word/document.xml")!.Open()).ReadToEnd();
            Assert.Contains("What moved", body);
        }
        using (var deck = new ZipArchive(new MemoryStream(files.Presentation)))
            Assert.NotNull(deck.GetEntry("ppt/presentation.xml"));
        Assert.True(files.Slides >= 1);
    }

    /// <summary>The migration's "after": the invoice the "before" printed with Chrome, its script run.</summary>
    [Fact]
    public async Task TheMigratedInvoice_PrintsWithItsScript()
    {
        var html = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "invoice.html"));
        var (text, pages) = Read(await Invoices.PdfAsync(html));

        Assert.Contains("NorthwindStudio", Squash(text));
        Assert.Contains("12,250.00", text);
        Assert.Contains("PrintedFriNov202026.", Squash(text));
        Assert.InRange(pages[0].Width, 594, 597);
    }
}
