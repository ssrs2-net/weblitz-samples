// One HTML page as a PDF, an Excel workbook, a Word document and a PowerPoint deck, in out/.
using WeblitzPdf.Samples.OfficeExport;

var html = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "report.html"));
var files = await Export.AllAsync(html);
var output = Directory.CreateDirectory("out").FullName;
await File.WriteAllBytesAsync(Path.Combine(output, "report.pdf"), files.Pdf);
await File.WriteAllBytesAsync(Path.Combine(output, "report.xlsx"), files.Workbook);
await File.WriteAllBytesAsync(Path.Combine(output, "report.docx"), files.Document);
await File.WriteAllBytesAsync(Path.Combine(output, "report.pptx"), files.Presentation);
Console.WriteLine($"out/report.pdf ({files.Pages} pages), report.xlsx, report.docx, report.pptx ({files.Slides} slides)");
