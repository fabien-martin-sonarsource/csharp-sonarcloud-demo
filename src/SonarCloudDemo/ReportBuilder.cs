namespace SonarCloudDemo;

public static class ReportBuilder
{
    // Intentional: near-duplicate of BuildDetailedSummary below, to trigger
    // the duplicated-code detector.
    public static string BuildSummary(string period, double revenue, double cost)
    {
        double margin = revenue - cost;
        double marginPercent = margin / revenue * 100;
        string unusedLabel = "draft"; // Intentional: unused variable (S1481).

        var lines = new List<string>
        {
            $"Period: {period}",
            $"Revenue: {revenue:C}",
            $"Cost: {cost:C}",
            $"Margin: {margin:C}",
            $"Margin %: {marginPercent:F2}%"
        };

        return string.Join(Environment.NewLine, lines);
    }

    public static string BuildDetailedSummary(string period, double revenue, double cost)
    {
        double margin = revenue - cost;
        double marginPercent = margin / revenue * 100;

        var lines = new List<string>
        {
            $"Period: {period}",
            $"Revenue: {revenue:C}",
            $"Cost: {cost:C}",
            $"Margin: {margin:C}",
            $"Margin %: {marginPercent:F2}%",
            $"Details for: {period}"
        };

        return string.Join(Environment.NewLine, lines);
    }
}
