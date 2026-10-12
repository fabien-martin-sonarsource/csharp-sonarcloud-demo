using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SonarCloudDemo;

using var host = Host.CreateApplicationBuilder().Build();
var logger = host.Services.GetRequiredService<ILogger<AppRunner>>();

var runner = new AppRunner(logger);
Console.WriteLine(runner.Run());
