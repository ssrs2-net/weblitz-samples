using System.Globalization;

namespace WeblitzPdf.Samples.Templates.Data;

public sealed record Party(string Name, IReadOnlyList<string> Address, string? Email = null, string? TaxNumber = null);

public sealed record Line(string Description, decimal Quantity, decimal UnitPrice)
{
    public decimal Amount => Quantity * UnitPrice;
}

public sealed record InvoiceData(string Number, DateOnly Date, DateOnly Due, Party Seller, Party Buyer, IReadOnlyList<Line> Lines,
    decimal TaxRate, string Currency, string Iban, string Note)
{
    public decimal Subtotal => Lines.Sum(l => l.Amount);
    public decimal Tax => Math.Round(Subtotal * TaxRate, 2);
    public decimal Total => Subtotal + Tax;
}

public sealed record ReceiptData(string Store, IReadOnlyList<string> StoreAddress, string Number, DateTime At, IReadOnlyList<Line> Items,
    decimal TaxRate, string Currency, string PaidWith)
{
    public decimal Total => Items.Sum(i => i.Amount);
    public decimal TaxIncluded => Math.Round(Total - Total / (1 + TaxRate), 2);
}

public sealed record Transaction(DateOnly Date, string Description, decimal Amount);

public sealed record StatementData(string Bank, Party Holder, string Account, DateOnly From, DateOnly To, decimal Opening,
    IReadOnlyList<Transaction> Transactions, string Currency)
{
    public decimal Credits => Transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
    public decimal Debits => -Transactions.Where(t => t.Amount < 0).Sum(t => t.Amount);
    public decimal Closing => Opening + Credits - Debits;
}

public sealed record CertificateData(string Title, string Recipient, string Achievement, DateOnly Date, string Issuer,
    string Signatory, string SignatoryRole, string Id);

public sealed record ShippingLabelData(Party From, Party To, string Carrier, string Service, string Tracking, decimal WeightKg,
    string Reference, DateOnly Shipped);

/// <summary>How the templates write amounts and dates: invariant, so a sample prints alike on every machine.</summary>
public static class Format
{
    public static string Money(decimal amount) => amount.ToString("#,##0.00", CultureInfo.InvariantCulture);
    public static string Date(DateOnly date) => date.ToString("d MMMM yyyy", CultureInfo.InvariantCulture);
    public static string ShortDate(DateOnly date) => date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
}
