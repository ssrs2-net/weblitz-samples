namespace WeblitzPdf.Samples.Templates.Data;

/// <summary>Made-up data for the templates: no real company, person or account.</summary>
public static class SampleData
{
    private static readonly Party Studio = new("Northwind Studio Ltd", ["Rue du Rhône 12", "1204 Geneva", "Switzerland"], "billing@northwind.example", "CHE-123.456.789 VAT");
    private static readonly Party Customer = new("Fenwick & Moor Logistics", ["Attn. Accounts payable", "48 Harbour Road", "Leith EH6 6LX", "United Kingdom"], "ap@fenwickmoor.example");

    public static InvoiceData Invoice => new(
        Number: "2026-0412",
        Date: new DateOnly(2026, 11, 20),
        Due: new DateOnly(2026, 12, 20),
        Seller: Studio,
        Buyer: Customer,
        Lines:
        [
            new("Discovery workshop (2 days, on site)", 2, 1800m),
            new("Design system: tokens, components, documentation", 1, 6400m),
            new("Invoice and statement templates (HTML, print CSS)", 5, 450m),
            new("Accessibility review of the customer portal", 1, 1950m),
            new("Hosting and monitoring, November", 1, 240m),
            new("Travel expenses (Geneva to Edinburgh, return)", 1, 612.40m),
        ],
        TaxRate: 0.081m,
        Currency: "CHF",
        Iban: "CH93 0076 2011 6238 5295 7",
        Note: "Payable within 30 days. Please quote the invoice number with your payment.");

    public static ReceiptData Receipt => new(
        Store: "Café Lumière",
        StoreAddress: ["Place du Molard 3", "1204 Geneva"],
        Number: "R-58213",
        At: new DateTime(2026, 11, 20, 8, 42, 0),
        Items:
        [
            new("Flat white", 2, 5.20m),
            new("Croissant au beurre", 2, 3.10m),
            new("Pain au chocolat", 1, 3.40m),
            new("Orange juice, fresh", 1, 6.50m),
            new("Granola bowl", 1, 9.80m),
        ],
        TaxRate: 0.026m,
        Currency: "CHF",
        PaidWith: "Card ending 4417, contactless");

    public static StatementData Statement
    {
        get
        {
            // One month of a current account: rent and the monthly bills early, the salary on the 25th, the rest as it comes
            var month = new (int Day, string Text, decimal Amount)[]
            {
                (1, "Rent, Rue des Alpes 7", -2150.00m), (1, "Gym membership", -79.00m), (2, "Train pass, CFF", -330.00m),
                (2, "Groceries, Migros Plainpalais", -84.35m), (3, "Coffee, Café Lumière", -12.40m), (3, "Electricity, SIG", -96.20m),
                (4, "Bookshop, Payot", -41.00m), (5, "Mobile phone, Swisscom", -59.00m), (5, "Groceries, Coop Rive", -112.75m),
                (6, "Pharmacy, Amavita", -23.90m), (7, "Coffee, Café Lumière", -9.80m), (8, "Restaurant, Les Halles", -126.50m),
                (8, "Groceries, Migros Plainpalais", -67.10m), (9, "Health insurance, Groupe Mutuel", -412.60m), (10, "Cinema, Pathé Balexert", -38.00m),
                (11, "Groceries, Coop Rive", -94.20m), (12, "Coffee, Café Lumière", -14.60m), (12, "Hardware, Jumbo", -58.45m),
                (13, "Refund, online shop", 64.90m), (14, "Groceries, Migros Plainpalais", -103.80m), (15, "Bakery, Au Pain Doré", -11.20m),
                (15, "Restaurant, Café du Soleil", -88.00m), (16, "Pharmacy, Amavita", -17.50m), (17, "Groceries, Coop Rive", -76.95m),
                (18, "Concert tickets, Victoria Hall", -120.00m), (19, "Coffee, Café Lumière", -9.80m), (20, "Groceries, Migros Plainpalais", -88.60m),
                (20, "Laundry, Lavomatic", -16.00m), (21, "Bike repair, Cycles du Rhône", -64.00m), (22, "Groceries, Coop Rive", -102.30m),
                (23, "Restaurant, Les Halles", -74.50m), (24, "Bookshop, Payot", -29.90m), (25, "Salary, Fenwick & Moor Logistics", 6850.00m),
                (25, "Transfer to savings", -1000.00m), (26, "Groceries, Migros Plainpalais", -91.15m), (26, "Coffee, Café Lumière", -12.40m),
                (27, "Train ticket, Geneva to Bern", -52.00m), (27, "Restaurant, Bern station", -31.80m), (28, "Groceries, Coop Rive", -84.05m),
                (28, "Pharmacy, Amavita", -12.90m), (29, "Cinema, Pathé Balexert", -38.00m), (29, "Gift, Manor", -79.90m),
                (30, "Groceries, Migros Plainpalais", -97.25m), (30, "Coffee, Café Lumière", -9.80m), (31, "Restaurant, Café du Soleil", -102.00m),
                (31, "Account fee", -5.00m),
            };
            var transactions = month.Select(t => new Transaction(new DateOnly(2026, 10, t.Day), t.Text, t.Amount)).ToList();
            return new StatementData("Banque du Léman", new Party("Ada Marchetti", ["Rue des Alpes 7", "1201 Geneva"]), "CH56 0483 5012 3456 7800 9",
                new DateOnly(2026, 10, 1), new DateOnly(2026, 10, 31), 6210.55m, transactions, "CHF");
        }
    }

    public static CertificateData Certificate => new(
        Title: "Certificate of completion",
        Recipient: "Ada Marchetti",
        Achievement: "has completed the course Print-ready HTML: paged media, fonts and PDF, with distinction",
        Date: new DateOnly(2026, 11, 20),
        Issuer: "Northwind Studio Academy",
        Signatory: "Jonas Keller",
        SignatoryRole: "Head of training",
        Id: "NSA-2026-1187");

    public static ShippingLabelData Label => new(
        From: Studio,
        To: new Party("Fenwick & Moor Logistics", ["Goods in, dock 4", "48 Harbour Road", "Leith EH6 6LX", "United Kingdom"]),
        Carrier: "Swift Parcel",
        Service: "EXPRESS 24",
        Tracking: "SP4417203GB",
        WeightKg: 2.4m,
        Reference: "PO 88123 / INV 2026-0412",
        Shipped: new DateOnly(2026, 11, 20));
}
