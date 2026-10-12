using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace SonarCloudDemo.Tests;

public class AppRunnerTests
{
    [Fact]
    public void Run_ReturnsJsonPayloadContainingBothReports()
    {
        var runner = new AppRunner(NullLogger<AppRunner>.Instance);

        var payload = runner.Run();

        var json = JObject.Parse(payload);
        var report = json.Value<string>("report") ?? string.Empty;
        var report2 = json.Value<string>("report2") ?? string.Empty;

        Assert.Contains("Period: Q1", report);
        Assert.Contains("Details for: Q2", report2);
    }
}
