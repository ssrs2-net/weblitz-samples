using WeblitzPdf.Samples.Templates.Components;
using WeblitzPdf.Samples.Templates.Data;

namespace WeblitzPdf.Samples.Templates;

/// <summary>Every template with its sample data, as HTML.</summary>
public static class Documents
{
    public static readonly IReadOnlyList<string> Names = ["invoice", "receipt", "bank-statement", "certificate", "shipping-label"];

    public static Task<string> HtmlAsync(string name) => name switch
    {
        "invoice" => DocumentRenderer.HtmlAsync<Invoice>(new Dictionary<string, object?> { ["Data"] = SampleData.Invoice }),
        "receipt" => DocumentRenderer.HtmlAsync<Receipt>(new Dictionary<string, object?> { ["Data"] = SampleData.Receipt }),
        "bank-statement" => DocumentRenderer.HtmlAsync<BankStatement>(new Dictionary<string, object?> { ["Data"] = SampleData.Statement }),
        "certificate" => DocumentRenderer.HtmlAsync<Certificate>(new Dictionary<string, object?> { ["Data"] = SampleData.Certificate }),
        "shipping-label" => DocumentRenderer.HtmlAsync<ShippingLabel>(new Dictionary<string, object?> { ["Data"] = SampleData.Label }),
        _ => throw new ArgumentException($"No template named {name}", nameof(name)),
    };
}
