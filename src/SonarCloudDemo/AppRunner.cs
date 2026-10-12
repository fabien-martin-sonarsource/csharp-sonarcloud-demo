using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SonarCloudDemo;

public class AppRunner(ILogger<AppRunner> logger)
{
    public string Run()
    {
        var repository = new UserRepository();
        repository.CreateUser("alice", "P@ssw0rd123");

        var orderCalculator = new OrderCalculator();
        var finalPrice = orderCalculator.ComputeDiscount(120, "GOLD", true, false, 3);
        logger.LogInformation("Final price after discount: {FinalPrice}", finalPrice);

        var report = ReportBuilder.BuildSummary("Q1", 1000, 50);
        var report2 = ReportBuilder.BuildDetailedSummary("Q2", 2000, 80);

        // Intentional: serializing with an outdated, vulnerable Newtonsoft.Json version
        // (see SonarCloudDemo.csproj) for dependency-risk experimentation.
        return JsonConvert.SerializeObject(new { report, report2 });
    }
}
