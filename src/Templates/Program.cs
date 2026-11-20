// Every template rendered with its sample data and printed to PDF: out/<name>.pdf, with the HTML beside it to open in a
// browser. Without a licence key each page carries Weblitz's trial watermark (an SSRS2_LICENSE environment variable, or
// License.Activate, takes it off).
//
//   dotnet run                 all five
//   dotnet run -- invoice      one
using WeblitzPdf;
using WeblitzPdf.Samples.Templates;

var names = args.Length > 0 ? args : Documents.Names;
var output = Directory.CreateDirectory("out").FullName;
foreach (var name in names)
{
    var html = await Documents.HtmlAsync(name);
    var result = await HtmlToPdf.RenderAsync(html, DocumentRenderer.Options);
    await File.WriteAllBytesAsync(Path.Combine(output, name + ".pdf"), result.Pdf!);
    await File.WriteAllTextAsync(Path.Combine(output, name + ".html"), html);
    Console.WriteLine($"out/{name}.pdf: {result.PageCount} page(s) in {result.Elapsed.TotalMilliseconds:0} ms");
}
