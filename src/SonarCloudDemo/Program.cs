using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SonarCloudDemo;

using var host = Host.CreateApplicationBuilder().Build();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

var repository = new UserRepository();
repository.CreateUser("alice", "P@ssw0rd123");

var orderCalculator = new OrderCalculator();
var finalPrice = orderCalculator.ComputeDiscount(120, "GOLD", true, false, 3);
logger.LogInformation("Final price after discount: {FinalPrice}", finalPrice);

var report = ReportBuilder.BuildSummary("Q1", 1000, 50);
var report2 = ReportBuilder.BuildDetailedSummary("Q2", 2000, 80);

// Intentional: serializing with an outdated, vulnerable Newtonsoft.Json version
// (see SonarCloudDemo.csproj) for dependency-risk experimentation.
var payload = JsonConvert.SerializeObject(new { report, report2 });
Console.WriteLine(payload);
