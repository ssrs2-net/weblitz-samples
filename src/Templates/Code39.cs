using System.Globalization;
using System.Text;

namespace WeblitzPdf.Samples.Templates;

/// <summary>
/// A Code 39 barcode as SVG: each character nine elements (bar, space, bar...), three of them wide, a narrow space
/// between characters, the text framed by the start and stop character (*). Upper-case letters, digits and - . space $ / + %.
/// </summary>
public static class Code39
{
    // 1 = a wide element, 0 = a narrow one, bars and spaces alternating from a bar
    private static readonly Dictionary<char, string> Patterns = new()
    {
        ['0'] = "000110100", ['1'] = "100100001", ['2'] = "001100001", ['3'] = "101100000", ['4'] = "000110001",
        ['5'] = "100110000", ['6'] = "001110000", ['7'] = "000100101", ['8'] = "100100100", ['9'] = "001100100",
        ['A'] = "100001001", ['B'] = "001001001", ['C'] = "101001000", ['D'] = "000011001", ['E'] = "100011000",
        ['F'] = "001011000", ['G'] = "000001101", ['H'] = "100001100", ['I'] = "001001100", ['J'] = "000011100",
        ['K'] = "100000011", ['L'] = "001000011", ['M'] = "101000010", ['N'] = "000010011", ['O'] = "100010010",
        ['P'] = "001010010", ['Q'] = "000000111", ['R'] = "100000110", ['S'] = "001000110", ['T'] = "000010110",
        ['U'] = "110000001", ['V'] = "011000001", ['W'] = "111000000", ['X'] = "010010001", ['Y'] = "110010000",
        ['Z'] = "011010000", ['-'] = "010000101", ['.'] = "110000100", [' '] = "011000100", ['*'] = "010010100",
        ['$'] = "010101000", ['/'] = "010100010", ['+'] = "010001010", ['%'] = "000101010",
    };

    public static IReadOnlyDictionary<char, string> Table => Patterns;

    /// <summary>The barcode of <paramref name="text"/> as an SVG element: narrow elements one unit, wide three, quiet
    /// zones of ten units, drawn as tall as asked.</summary>
    public static string Svg(string text, double height = 60)
    {
        var symbols = "*" + text.ToUpperInvariant() + "*";
        var bars = new StringBuilder();
        double x = 10;
        foreach (var c in symbols)
        {
            if (!Patterns.TryGetValue(c, out var pattern))
                throw new ArgumentException($"Code 39 has no '{c}'", nameof(text));
            for (var i = 0; i < 9; i++)
            {
                var width = pattern[i] == '1' ? 3 : 1;
                if (i % 2 == 0)
                    bars.Append(CultureInfo.InvariantCulture, $"<rect x=\"{x}\" y=\"0\" width=\"{width}\" height=\"{height}\"/>");
                x += width;
            }
            x += 1; // the narrow gap between characters
        }
        var total = x - 1 + 10;
        return string.Create(CultureInfo.InvariantCulture,
            $"<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 {total} {height}\" preserveAspectRatio=\"none\" role=\"img\" aria-label=\"Barcode {text}\" style=\"width:100%;height:{height}px;display:block\"><g fill=\"#000\" shape-rendering=\"crispEdges\">{bars}</g></svg>");
    }
}
