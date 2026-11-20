namespace WeblitzPdf.Samples.OfficeExport;

/// <summary>One page as four files: the PDF, its tables as an Excel workbook, its content as a Word document to go on
/// writing in, its printed pages as PowerPoint slides. No Office on the machine.</summary>
public static class Export
{
    public sealed record Files(byte[] Pdf, byte[] Workbook, byte[] Document, byte[] Presentation, int Pages, int Slides);

    public static async Task<Files> AllAsync(string html)
    {
        var pdf = await HtmlToPdf.RenderAsync(html);
        var workbook = await HtmlToExcel.RenderAsync(html);
        var document = await HtmlToWord.RenderAsync(html);
        var deck = await HtmlToPowerPoint.RenderAsync(html);
        return new Files(pdf.Pdf!, workbook.Workbook!, document.Document!, deck.Presentation!, pdf.PageCount, deck.SlideCount);
    }
}
