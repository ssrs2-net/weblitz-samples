// After: HTML to PDF with Weblitz. Without a licence key every page carries the trial watermark.
using WeblitzPdf.Samples.PuppeteerMigration.After;

var html = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "invoice.html"));
var pdf = await Invoices.PdfAsync(html);
await File.WriteAllBytesAsync("invoice.pdf", pdf);
Console.WriteLine($"invoice.pdf: {pdf.Length / 1024} KB");
